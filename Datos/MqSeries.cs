using IBM.WMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
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
            using (MQQueueManager queueManager = new MQQueueManager(strQueueManagerName))
            {
                return true;
            }

        }

        public static bool MQEscrituraCola(string strQueueManagerName , string strInputMsg, string QueueName, MQOPEN lngOpciones, long lnglMQExpira)
        {
            try
            {
                using (MQQueueManager queueManager = new MQQueueManager(strQueueManagerName))
                {               
                    queue = queueManager.AccessQueue(QueueName, (int)lngOpciones);
                    message = strInputMsg;
                    queueMessage = new MQMessage();
                    queueMessage.ClearMessage();
                    queueMessage.Format = "MQSTR   ";
                    queueMessage.MessageType = MQC.MQMT_DATAGRAM;
                    queueMessage.WriteLong(strInputMsg.Length);
                    queueMessage.WriteString(message);

                    queuePutMessageOptions = new MQPutMessageOptions();
                    queuePutMessageOptions.Options = queuePutMessageOptions.Options | MQC.MQPMO_NO_SYNCPOINT;

                    queueMessage.CorrelationId = Encoding.ASCII.GetBytes(strMessageId);
                    queueMessage.ReplyToQueueName = QueueName;
                    queueMessage.Expiry = Convert.ToInt32(lnglMQExpira);
                    strCorrelId = queueMessage.CorrelationId.ToString();
                    queue.Put(queueMessage, queuePutMessageOptions);
                    return true;
                }
            }
            catch (MQException MQexp)
            {
                strCadenaLogMQ = "error escritura " + MQexp.ReasonCode + " , " + MQexp.Message;
                return false;
            }
        }



        public static string MQAbrirCola(string strQueueManagerName, string QueueName,  MQOPEN lngOpciones)
        {
            
                string resultado = String.Empty;
                try
                {
                    using (MQQueueManager queueManager = new MQQueueManager(strQueueManagerName))
                    {
                        queue = queueManager.AccessQueue(QueueName, (int)lngOpciones);
                        queueMessage = new MQMessage();
                        queueMessage.Format = MQC.MQFMT_STRING;
                        //Se accesan a la opciones de lectura por default
                        queueGetMessageOptions = new MQGetMessageOptions();
                        queueGetMessageOptions.Options = MQGMO_NO_WAIT + MQGMO_COMPLETE_MSG;
                        queue.Get(queueMessage, queueGetMessageOptions);



                        //Obtener el Id del mensage para el regreso
                        msgID = queueMessage.MessageId;
                        strMessageId = Encoding.ASCII.GetString(msgID);
                        strCorrelId = queueMessage.CorrelationId.ToString();

                        resultado = queueMessage.ReadString(queueMessage.MessageLength);
                    }
                }
                catch (MQException MQexp)
                {

                    strCadenaLogMQ = "Error al leer Queue " + MQexp.Reason + " " +
                        MQexp.InnerException + " , " + MQexp.TargetSite + " , " + MQexp.Data +
                        +MQexp.ReasonCode + ", mensaje " + MQexp.Source + "  Error " + MQexp.Message;
                    queueManager.Close();
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
            string resultado = MQAbrirCola(strCola, strQueueManagerName, MQOPEN.MQOO_OUTPUT);

            if (resultado != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
      

    }
}
