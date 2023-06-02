using IBM.WMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ConexionMQ
    {
        //Declaraciones de los objetos para MQSeries
        private MQQueueManager queueManager;
        private MQQueue queue;
        private MQMessage queueMessage;
        private MQPutMessageOptions queuePutMessageOptions;
        private MQGetMessageOptions queueGetMessageOptions;

        private static string QueueManagerName;
        private static string ChannelInfo;
        private string channelName;
        private string transportType;
        private string connectionName;
        private string message;

        public string strCadenaLog;
        public long lngContadorErr;

        public string strMessageId; //Almacena el Id del mensaje recibido
        public byte[] msgID;
        public string strCorrelId;  //Almacena el Correlación ID

        public const int MQGMO_NO_WAIT = 0;
        public const int MQGMO_COMPLETE_MSG = 65536;

        string nl = Environment.NewLine;
        public string strCadenaLogMQ;
        public MQException error;


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





        /// <summary>
        /// Realiza la conexión al MQ
        /// </summary>
        /// <param name="strQueueManagerName"></param>
        /// <param name="objMQManager"></param>
        /// <returns></returns>
        public MQQueueManager MQConectar(string strQueueManagerName)
        {
            QueueManagerName = strQueueManagerName;     
            queueManager = new MQQueueManager(QueueManagerName);
            return queueManager;          
        }



        /// <summary>
        /// escribe mensaje de la queu  
        /// </summary>
        /// <param name="strInputMsg"></param>
        /// <param name="QueueName"></param>
        /// <param name="queueManager"></param>
        /// <param name="lngOpciones"></param>
        /// <param name="lnglMQExpira"></param>
        /// <returns></returns>
        public bool MQEscrituraCola(string strInputMsg, string QueueName, MQQueueManager queueManager, MQOPEN lngOpciones, long lnglMQExpira)
        {
            try
            {

                queue = queueManager.AccessQueue(QueueName, (int)lngOpciones);
                message = strInputMsg;
                queueMessage = new MQMessage();
                queueMessage.ClearMessage();
                queueMessage.Format = "MQSTR   ";
                queueMessage.MessageType = MQC.MQMT_DATAGRAM;
                queueMessage.WriteLong(strInputMsg.Length);
                queueMessage.WriteString(message);

                //queueMessage.mes = strInputMsg;
                queuePutMessageOptions = new MQPutMessageOptions();
                queuePutMessageOptions.Options = queuePutMessageOptions.Options | MQC.MQPMO_NO_SYNCPOINT;


                //Asigno el Id del mensaje que obtuve anteriormente
                queueMessage.CorrelationId = Encoding.ASCII.GetBytes(strMessageId);
                queueMessage.ReplyToQueueName = QueueName;
                queueMessage.Expiry = Convert.ToInt32(lnglMQExpira);
                strCorrelId = queueMessage.CorrelationId.ToString();
                queue.Put(queueMessage, queuePutMessageOptions);
                return true;
            }
            catch (MQException MQexp)
            {
                strCadenaLogMQ = "error escritura " + MQexp.ReasonCode + " , " + MQexp.Message;
                return false;
            }


        }

        /// <summary>
        /// Lee mensaje de la queu   
        /// </summary>
        /// <param name="QueueName"></param>
        /// <param name="queueManager"></param>
        /// <param name="lngOpciones"></param>
        /// <param name="strReturn"></param>
        /// <returns></returns>
        public string MQAbrirCola(string QueueName, MQQueueManager queueManager, MQOPEN lngOpciones)
        {
            string resultado = String.Empty;

            try
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

        public bool MQVerificar(string strCola)
        {
            string resultado = MQAbrirCola(strCola, queueManager, MQOPEN.MQOO_OUTPUT);
            
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
