using IBM.WMQ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloNegocio
{
    public static class MqSeries
    {
        private static MQQueueManager queueManager;
        private static MQQueue queue;
        private static MQMessage queueMessage;
        private static MQPutMessageOptions queuePutMessageOptions;
        private static MQGetMessageOptions queueGetMessageOptions;

        private static string QueueManagerName;
        private static string ChannelInfo;
        private static string channelName;
        private static string transportType;
        private static string connectionName;
        private static string message;

        public static string strCadenaLog;
        public static long lngContadorErr;

        public static string strMessageId; //Almacena el Id del mensaje recibido
        public static byte[] msgID;
        public static string strCorrelId;  //Almacena el Correlación ID

        public const int MQGMO_NO_WAIT = 0;
        public const int MQGMO_COMPLETE_MSG = 65536;

        static string nl = Environment.NewLine;
        public static string strCadenaLogMQ;
        public static MQException error;

        /// <summary>
        /// Enumeración para las opciones de abrir la cola
        /// </summary>
        public enum MQOPEN
        {

            MQOO_INPUT_AS_Q_DEF = 0x1,
            MQOO_INPUT_SHARED = 0x2,
            MQOO_INPUT_EXCLUSIVE = 0x4,
            MQOO_BROWSE = 0x8,
            MQOO_OUTPUT = 0x10,
            MQOO_INQUIRE = 0x20,
            MQOO_SET = 0x40,
            MQOO_BIND_ON_OPEN = 0x4000,
            MQOO_BIND_NOT_FIXED = 0x8000,
            MQOO_BIND_AS_Q_DEF = 0x0,
            MQOO_SAVE_ALL_CONTEXT = 0x80,
            MQOO_PASS_IDENTITY_CONTEXT = 0x100,
            MQOO_PASS_ALL_CONTEXT = 0x200,
            MQOO_SET_IDENTITY_CONTEXT = 0x400,
            MQOO_SET_ALL_CONTEXT = 0x800,
            MQOO_ALTERNATE_USER_AUTHORITY = 0x1000,
            MQOO_FAIL_IF_QUIESCING = 0x2000
        }

        public enum TipoAccion
        {
            eMQConectar = 0,
            eMQDesconectar = 1,
            eMQAbrirCola = 2,
            eMQCerrarCola = 3,
            eMQLeerCola = 4,
            eMQEscribirCola = 5,
            eMQOtro = 6,
        }

        /// <summary>
        /// Prueba que se peuda conectar al MQ
        /// </summary>
        /// <param name="strQueueManagerName">Nombre Queue</param>
        /// <returns></returns>
        public static bool PruebaConexion(string strQueueManagerName)
        {
            try
            {
                Log.Escribe("conectando....");
                using (MQQueueManager queueManager = new MQQueueManager(strQueueManagerName))
                {
                    Log.Escribe("CONECTADO");
                    return true;
                }
            }
            catch (MQException MQexp)
            {
                Log.Escribe(MQexp);
                Ticket.BitacoraErrorMapeoSave(MQexp.ReasonCode, MQexp.Message, "", TipoAccion.eMQConectar);
                return false;
            }


        }

        public static bool MQEscrituraCola(MQQueueManager queueManager, string QueueName, MQOPEN lngOpciones, List<string> mensajes, long lnglMQExpira = 0)
        {
            try
            {
                foreach (string mensaje in mensajes)
                {
                    Log.Escribe("Escribir mensaje en la cola : " + QueueName);
                    Log.Escribe(mensaje);
                    queue = queueManager.AccessQueue(QueueName, (int)lngOpciones);

                    queueMessage = new MQMessage();
                    queueMessage.ClearMessage();
                    queueMessage.Format = MQC.MQFMT_STRING;
                    queueMessage.MessageType = MQC.MQMT_DATAGRAM;
                    queueMessage.WriteString(mensaje.Trim());

                    queuePutMessageOptions = new MQPutMessageOptions();
                    queuePutMessageOptions.Options = queuePutMessageOptions.Options | MQC.MQPMO_NO_SYNCPOINT | MQC.MQPMO_DEFAULT_CONTEXT;

                    //queueMessage.CorrelationId = Encoding.ASCII.GetBytes(strMessageId);
                    //queueMessage.ReplyToQueueName = QueueName;
                    //queueMessage.Expiry = Convert.ToInt32(lnglMQExpira);
                    //strCorrelId = queueMessage.CorrelationId.ToString();
                    queue.Put(queueMessage, queuePutMessageOptions);
                    Log.Escribe(mensaje);
                    Log.Escribe("**********| Mensaje ponido jejeje XD XD |**********"); 
                }

                return true;
            }
            catch (MQException MQexp)
            {
                Log.Escribe(MQexp);
                Ticket.BitacoraErrorMapeoSave(MQexp.ReasonCode, MQexp.Message, "", TipoAccion.eMQEscribirCola);
                return false;
            }
        }



        public static string MQLecturaCola(MQQueueManager queueManager, string strMqCola, MQOPEN lngOpciones)
        {

            string resultado = String.Empty;
            try
            {
                if(queueManager.IsOpen)
                {
                    Log.Escribe("ABIERTO");
                }
                else
                {
                    Log.Escribe("CERRADO");
                }

                Log.Escribe("Abriendo cola...");
             
                queue = queueManager.AccessQueue(strMqCola, (int)lngOpciones);
                Log.Escribe("1...");
                queueMessage = new MQMessage();
                Log.Escribe("2...");
                queueMessage.Format = MQC.MQFMT_STRING;
                Log.Escribe("3...");
                //Se accesan a la opciones de lectura por default
                queueGetMessageOptions = new MQGetMessageOptions();
                Log.Escribe("4...");
                queueGetMessageOptions.Options = MQGMO_NO_WAIT + MQGMO_COMPLETE_MSG;
                Log.Escribe("5...");
                queue.Get(queueMessage, queueGetMessageOptions);



                //Obtener el Id del mensage para el regreso
                msgID = queueMessage.MessageId;
                strMessageId = Encoding.ASCII.GetString(msgID);
                strCorrelId = queueMessage.CorrelationId.ToString();

                resultado = queueMessage.ReadString(queueMessage.MessageLength);
                
            }
            catch (MQException MQexp)
            {
                Log.Escribe(MQexp);
                Ticket.BitacoraErrorMapeoSave(MQexp.ReasonCode, MQexp.Message, "", TipoAccion.eMQAbrirCola);
                return String.Empty;
            }
            finally
            {
                queue.Close();
            }

            return resultado;

        }

        public static bool MQVerificar(string strQueueManagerName, string strCola)
        {
            //string resultado = MQAbrirCola(strCola, strQueueManagerName, MQOPEN.MQOO_OUTPUT);

            //if (resultado != "")
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}

            return false;
        }

        public static bool MQEnviar(string strQueueManagerName, string strMqCola, string RutaArchivo, string strMnesajeId = "")
        {
            try
            {
                using (MQQueueManager queueManager = new MQQueueManager(strQueueManagerName))
                {
                    List<string> mensajes = new List<string>();
                    mensajes = GetMensajes(RutaArchivo);
                 
                    long longOPen = (long)MQOPEN.MQOO_OUTPUT;
                    bool resp = MQEscrituraCola(queueManager, strMqCola, (MQOPEN)longOPen, mensajes, (long)0);
                }
                
                return true;
            }
            catch (MQException MQexp)
            {
                Log.Escribe(MQexp);
                Ticket.BitacoraErrorMapeoSave(MQexp.ReasonCode, MQexp.Message, "", TipoAccion.eMQAbrirCola);
                return false;
            }
            
        }

        private static List<string> GetMensajes(string rutaArchivo)
        {
            using (StreamReader reader = new StreamReader(rutaArchivo))
            {
                List<string> mensajes = new List<string>();

                string linea;
                string mensaje = "";

                while ((linea = reader.ReadLine()) != null)
                {
                    if (linea.Contains("$"))
                    {
                        linea = linea.Replace("$", "");
                        linea = linea.Replace("-}", "");
                        mensaje += "-}";
                        mensajes.Add(mensaje);
                        mensaje = "";
                    }
                    if (reader.EndOfStream)
                    {
                        mensaje += linea;
                        mensajes.Add(mensaje);
                        break;
                    }
                    mensaje += linea + Environment.NewLine;
                }

                return mensajes;
            }
        }
    }
}
