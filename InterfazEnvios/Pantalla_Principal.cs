using Datos;
using IBM.WMQ;
using MNICript;
using ModeloNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfazEnvios
{
    public partial class Pantalla_Principal : Form
    {
        clsEncripta crpt;

        public ConexionAS400 as400;      
        public Configuracion confg;
        public string mensajes;
        public bool gbMQConectado;

        string msSendCtes;
        string msSendTDs;
        string msSendDepRet;
        string msSendDepRetAj;
        string msSendDepRetTDD;
        string msSendROPD;
        string msSendTrasp;
        string msSendMT103;
        string msSendHOLD;
        string msSendSwift;
        string msSendTDOver;
        string msSendMT202;

        public DateTime gsFecOp1;
        public DateTime gsFecOp2;
        public DateTime gsFecOp3;
        public DateTime gsFecOp4;

        public DateTime gsFecResp;
        public DateTime gsFecSV1;
        public DateTime gsFecSV2;

        public bool mbTransStatus;

        public Datos.Parametros gsTotMT103;
        public Datos.Parametros gsTotMT202;
        public Datos.Parametros gsTotMT198;
        public Datos.Parametros gsTotClientes;
        public Datos.Parametros gsTotMantto;
        public Datos.Parametros gsTotDepRet;
        public Datos.Parametros gsTotHOLDRet;
        public Datos.Parametros gsTotROPUSD;
        public Datos.Parametros gsTotTraspasos;
        public Datos.Parametros gsTotTDs;

        public static Int16 gsPaso;
        public static Int16 gsBackup;

        //Variables para el control de Operaciones Parciales
        string gsMT103P = "0";
        string gsMT198P = "0";
        string gsMT202P = "0";
        string gsClientesP = "0";
        string gsManttoP = "0";
        string gsDepRetP = "0";
        string gsHOLDP = "0";
        string gsROPUSDP = "0";
        string gsTraspasosP = "0";
        string gsTDsP = "0";
        //Variables para el control Ultima Fecha de Envio
        string gsFecMT103U = "";
        string gsFecMT198U = "";
        string gsFecMT202U = "";
        string gsFecClientesU = "";
        string gsFecManttoU = "";
        string gsFecDepRetU = "";
        string gsFecHOLDU = "";
        string gsFecROPUSDU = "";
        string gsFecTraspasosU = "";
        string gsFecTDsU = "";
        //Variables para el control de Operaciones Enviadas a La Fecha
        string gsFecOp1U = "";
        string gsFecOp2U = "";
        string gsFecOp3U = "";
        string gsFecOp4U = "";
        //Variables para el monitoreo MERD, pantalla para monitoreo
        string gsFecDepSucU2 = "";
        string gsFecRetSucU2 = "";
        string gsFecTraspasosMAU = "";
        string gsFecTraspasosEAU = "";
        string gsFecDepTDDU2 = "";
        string gsFecRetTDDU2 = "";
        string gsRetOpeU2 = "";
        string gsOpeEspeU2 = "";
        string gsFecTDsHOUU = "";
        string gsFecTDsGCU = "";

        // VARIABLES PARA EL ENVIO DE XML
        string gsEnvioXML = "";
        string gsRutaEXEXML = "";
        string gsMQEscribirXML = "";
        string gsMQLeerXML = "";

        //SE USARA SÒLO CUANDO MODULO EXTRANJERO MANDE UN XML Y DEBAMOS GENERAR UN MT (HASTA QUE PAYPLUS PUEDA RECIBIR XML)
        string gsEnvioMT = "";

        public bool gbEncendido;
        public bool gbPasswordOK;
        public bool swith_click;


        //Variables utilizadas para almacenar el nombre de los archivos generados por la interfaz.
        //Estos nombres estan parametrizados y se obtienen de la base de datos.
        Datos.Parametros  gs_Base_Swift;    
        Datos.Parametros  gs_CL_AS400;      
        Datos.Parametros  gs_CL_Swift;
        Datos.Parametros gs_TD_AS400;
        Datos.Parametros gs_TD_Swift;
        Datos.Parametros gs_SWAG_Swift;
        Datos.Parametros gs_TRAN_Swift;
        Datos.Parametros gs_DEIB_AS400;
        Datos.Parametros gs_HOLD_AS400;
        Datos.Parametros gs_MT202_Swift;  
        Datos.Parametros gs_XML202_AMH;  
        Datos.Parametros gs_XMLTRAN_AMH;

        Datos.Parametros ls_StatusInterfaz;

        bool booNoChangeParam;
        bool booNoChangeConf;

        frmMonitoreo frm_monitor;

        public Pantalla_Principal()
        {
            InitializeComponent();

            crpt = new clsEncripta();
        }

        private void Pantalla_Principal_Resize(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Maximized)
            {
                pnlContainer.Width = (this.Width - pnlMenu.Width) - 10;
                pnlContainer.Height = (this.Height - pnlNavBar.Height) - 10;

                pnlMenu.Height = (this.Height - pnlNavBar.Height);

                pnlNavBar.Width = (this.Width);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {

            DialogResult dg = MessageBox.Show("Desea Salir de la Interfaz?", "Saliendo de IE", MessageBoxButtons.YesNo);

            if (dg == DialogResult.Yes)
            {
                gbPasswordOK = false;

                frmSolicPassword frm = new frmSolicPassword(this);
                frm.ShowDialog();


                if (!gbPasswordOK)
                {
                    return;
                }
                else
                {
                    this.Close();
                }               
            }
            else
            {
                return;
            } 
        }

        private void Pantalla_Principal_Load(object sender, EventArgs e)
        {           
            try
            {
                tsLblVersion.Text = "Interfaz Envios Version: " + Application.ProductVersion.ToString();
                tsLblMachineName.Text = "Maquina: " + Environment.MachineName;
                tsLblFechaPc.Text = "Fecha Actual: " + DateTime.Now.ToString("dd-MM-yyyy");                       
                mbTransStatus = true;

                switchButton1.Enabled = false;              

                if(InicializaVariables())
                {
                    long longOpen = (long)MqSeries.MQOPEN.MQOO_INPUT_AS_Q_DEF;
                    ModeloNegocio.MqSeries.MQLecturaCola(confg.mqManager, confg.mqLeer, (MqSeries.MQOPEN)longOpen);
                    this.Close();
                    return;


                    if (VerificaPaths())
                    {                     
                        if (((ModeloNegocio.MqSeries.PruebaConexion(confg.mqManager))))
                        {
                            if(nameFiles())
                            {
                                switchButton1.Enabled = true;
                                EncenderInterfaz(true);
                                switchButton1.Checked = true;
                            }
                            else
                            {
                                MessageBox.Show("Hubo un error en la configuracion de variables de nombre de archivos, Checar el LOG", "ERROR CONFIGURACION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Hubo un error en la conexion al MQ {confg.mqManager}, Checar el LOG", "ERROR CONFIGURACION", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                    else
                    {
                        MessageBox.Show("Hubo un error en la configuracion de directorios, Checar el LOG", "ERROR CONFIGURACION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Hubo un error en la configuracion de variables, Checar el LOG", "ERROR CONFIGURACION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                }

            }
            catch(MQException ex)
            {
                Log.Escribe(ex);
                MessageBox.Show("Hubo un error al Conectarse al MQ", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
                MessageBox.Show(ex.Message, "Hubo un error" , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

       

        private bool nameFiles()
        {
            try
            {
                gs_Base_Swift = ModeloNegocio.Parametro.GetParametrizacion("BASE_SWIFT");
                gs_CL_AS400 = ModeloNegocio.Parametro.GetParametrizacion("FILE_CL_AS400");
                gs_CL_Swift = ModeloNegocio.Parametro.GetParametrizacion("FILE_CL_SWIFT");
                gs_TD_AS400 = ModeloNegocio.Parametro.GetParametrizacion("FILE_TD_AS400");
                gs_TD_Swift = ModeloNegocio.Parametro.GetParametrizacion("FILE_TD_SWIFT");
                gs_SWAG_Swift = ModeloNegocio.Parametro.GetParametrizacion("FILE_SWAG_SWIFT");
                gs_TRAN_Swift = ModeloNegocio.Parametro.GetParametrizacion("FILE_TRAN_SWIFT");
                gs_DEIB_AS400 = ModeloNegocio.Parametro.GetParametrizacion("FILE_DEIB_AS400");
                gs_HOLD_AS400 = ModeloNegocio.Parametro.GetParametrizacion("FILE_HOLD_AS400");
                gs_MT202_Swift = ModeloNegocio.Parametro.GetParametrizacion("FILE_202_SWIFT");
                gs_XML202_AMH = ModeloNegocio.Parametro.GetParametrizacion("FILE_XML202_AMH");
                gs_XMLTRAN_AMH = ModeloNegocio.Parametro.GetParametrizacion("FILE_XMLTRAN_AMH");

                return true;
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
                return false;
            }          
        }

        private bool VerificaPaths()
        {
            try
            {
                if (!Directory.Exists(confg.pathEquation))
                {
                    Directory.CreateDirectory(confg.pathEquation);
                }
                if (!Directory.Exists(confg.pathSwift))
                {
                    Directory.CreateDirectory(confg.pathSwift);
                }
                if (!Directory.Exists(confg.pathModels))
                {
                    Directory.CreateDirectory(confg.pathModels);
                }
                if (!Directory.Exists(confg.pathFtpApp))
                {
                    Directory.CreateDirectory(confg.pathFtpApp);
                }
                if (!Directory.Exists(confg.pathSaldos))
                {
                    Directory.CreateDirectory(confg.pathSaldos);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
                return false;
            }
            
        }

        private bool InicializaVariables()
        {
            try
            {             
                int lnLongitud;
                int intValorTemporal;
                string strValorTemporal;
                DateTime dtValorTemporal;
                TimeSpan hrTemporal;

                confg = new Configuracion();
                confg.EncryptConnectionString();

                confg.AppName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                confg.gsAppPassword = crpt.VerificaClaves(2, confg.getValueAppConfig("APPPWD", "PARAMETRO"));

                Log.RutaLog = confg.getValueAppConfig("Ruta", "LOG");
                Log.EscribeLog = true;
                Log.Nombre_App = "InterfazEnvios";


                Log.Escribe("Inicia configuración de variables");

                DateTime fecha_Servidor = ModeloNegocio.Parametro.FechaServidor();
                ls_StatusInterfaz = ModeloNegocio.Parametro.GetParametrizacion("TRANSSTATUS");

                Datos.TICKETEntities bdTicket = new TICKETEntities();
                Datos.CATALOGOSEntities bdCatalogos = new CATALOGOSEntities();
                Datos.FUNCIONARIOSEntities bdFuncionario = new FUNCIONARIOSEntities();
                string ds1 = bdTicket.Database.Connection.DataSource;
                string ds2 = bdCatalogos.Database.Connection.DataSource;
                string ds3 = bdFuncionario.Database.Connection.DataSource;



                if (ds1 == ds2 && ds2 == ds3)
                {
                    DbConnection cnn = bdFuncionario.Database.Connection;
                    confg.usrSql = GetCredential(cnn, "id=");
                    confg.pswdSql = GetCredential(cnn, "password=");
                    confg.dataSource = ds3;

                    tsLblServer.Text = "Servidor:" + ds1;
                }
                else
                {

                    confg.usrSql = "";
                    confg.pswdSql = "";
                    confg.dataSource = ds1;

                    tsLblServer.Text = "Servidor: No todas la configuraciones apuntan al mismo servidor.";
                }

              
                confg.ambiente = confg.getValueAppConfig("AMBIENTE", "PARAMETRO");
                confg.pathFtpApp = confg.getValueAppConfig("PATHFTPAPP", "PARAMETRO");
                confg.sna620 = crpt.VerificaClaves(2, confg.getValueAppConfig("SNA620", "PARAMETRO"));
                confg.libSwift = crpt.VerificaClaves(2, confg.getValueAppConfig("LIBSWIFT", "PARAMETRO"));
                confg.libSaldos = crpt.VerificaClaves(2, confg.getValueAppConfig("LIBSALDOS", "PARAMETRO"));
                confg.libDefault = crpt.VerificaClaves(2, confg.getValueAppConfig("LIBDEFAULT", "PARAMETRO"));

                confg.mqManager = crpt.VerificaClaves(2, confg.getValueAppConfig("MQMANAGER", "PARAMETRO"));
                confg.mqEscribir = crpt.VerificaClaves(2, confg.getValueAppConfig("MQESCRIBIR", "PARAMETRO"));
                confg.mqLeer = crpt.VerificaClaves(2, confg.getValueAppConfig("MQLEER", "PARAMETRO"));
                confg.mqReporte = crpt.VerificaClaves(2, confg.getValueAppConfig("MQREPORTE", "PARAMETRO"));


                confg.sendLaops = confg.getValueAppConfig("SENDLAOPS", "PARAMETRO");
                
                if(confg.sendLaops.Length < 12)
                {
                    lnLongitud = 12 - (confg.sendLaops.Length);
                    confg.sendLaops += new string('1', lnLongitud); 
                }

                confg.sendGcOps = confg.getValueAppConfig("SENDGCOPS", "PARAMETRO");

                if (confg.sendGcOps.Length < 12)
                {
                    lnLongitud = 12 - (confg.sendGcOps.Length);
                    confg.sendGcOps += new string('1', lnLongitud);
                }

                confg.sendAS400 = confg.getValueAppConfig("SENDAS400", "PARAMETRO");
                confg.slashMT103 = confg.getValueAppConfig("SLASHMT103", "PARAMETRO");
                
                confg.chkEq = (confg.getValueAppConfig("CHKEQ", "PARAMETRO") == "1");
                confg.chkSw = (confg.getValueAppConfig("CHKSW", "PARAMETRO") == "1");
                confg.chkEnvEqt = (confg.getValueAppConfig("CHKENVEQT", "PARAMETRO") == "1");
                confg.chkEnvSwf = (confg.getValueAppConfig("CHKENVSWF", "PARAMETRO") == "1");
                confg.chkClose = (confg.getValueAppConfig("CHKCLOSE", "PARAMETRO") == "1");
                confg.chkSaldosHc = (confg.getValueAppConfig("CHKSALDOSHO", "PARAMETRO") == "1");
                confg.chkSaldosGc = (confg.getValueAppConfig("CHKSALDOSGC", "PARAMETRO") == "1");
                confg.chkTdsHo = (confg.getValueAppConfig("CHKTDSHO", "PARAMETRO") == "1");
                confg.chkTdsGc = (confg.getValueAppConfig("CHKTDSGC", "PARAMETRO") == "1");
                confg.chkOverGc = (confg.getValueAppConfig("CHKOVERGC", "PARAMETRO") == "1");

                intValorTemporal = Int32.Parse(confg.getValueAppConfig("PERIODO0", "PARAMETRO"));
                confg.periodo0 = (intValorTemporal < 2 || intValorTemporal > 30) ? 10 : intValorTemporal;

                intValorTemporal = Int32.Parse(confg.getValueAppConfig("PERIODO1", "PARAMETRO"));
                confg.periodo1 = (intValorTemporal < 2 || intValorTemporal > 30) ? 10 : intValorTemporal;

                intValorTemporal = Int32.Parse(confg.getValueAppConfig("PERIODO2", "PARAMETRO"));
                confg.periodo2 = (intValorTemporal < 2 || intValorTemporal > 30) ? 10 : intValorTemporal;

                intValorTemporal = Int32.Parse(confg.getValueAppConfig("PERIODO3", "PARAMETRO"));
                confg.periodo3 = (intValorTemporal < 2 || intValorTemporal > 30) ? 10 : intValorTemporal;
   
                intValorTemporal = Int32.Parse(confg.getValueAppConfig("PERIODORESP", "PARAMETRO"));
                confg.periodoResp = (intValorTemporal < 1 || intValorTemporal > 24) ? 10 : intValorTemporal;

                intValorTemporal = Int32.Parse(confg.getValueAppConfig("PERIODOSALDO0", "PARAMETRO"));
                confg.periodoSaldo0 = (intValorTemporal < 1 || intValorTemporal > 24) ? 10 : intValorTemporal;

                intValorTemporal = Int32.Parse(confg.getValueAppConfig("PERIODOSALDO1", "PARAMETRO"));
                confg.periodoSaldo1 = (intValorTemporal < 1 || intValorTemporal > 24) ? 10 : intValorTemporal;

                strValorTemporal =  confg.getValueAppConfig("FECHACIERRE", "PARAMETRO");
                confg.fechaCierre = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? fecha_Servidor : dtValorTemporal;

                strValorTemporal = confg.getValueAppConfig("RESPALDO", "PARAMETRO");
                confg.fechaRespaldo = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? Funciones.Fecha("FIN", confg.fechaCierre) : dtValorTemporal;

                strValorTemporal = confg.getValueAppConfig("SALDOFASE1", "PARAMETRO");
                confg.fechaSV1 = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? Funciones.Fecha("FIN", confg.fechaCierre) : dtValorTemporal;

                strValorTemporal = confg.getValueAppConfig("SALDOFASE2", "PARAMETRO");
                confg.fechaSV2 = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? Funciones.Fecha("FIN", confg.fechaCierre) : dtValorTemporal;

                if(mbTransStatus)
                {
                    strValorTemporal = confg.getValueAppConfig("INTERCLOSE", "PARAMETRO");
                    confg.horaCierre = (TimeSpan.TryParse(strValorTemporal, out hrTemporal)) ? hrTemporal : new TimeSpan(23, 0, 0); 
                }

                confg.fechaCierre = fecha_Servidor + confg.horaCierre;

                //Hora de cierre default cuando la interfaz se carga de forma automática
                strValorTemporal = confg.getValueAppConfig("INTERCLOSEAUTO", "PARAMETRO");
                confg.horaCierreAuto = (TimeSpan.TryParse(strValorTemporal, out hrTemporal)) ? fecha_Servidor  + hrTemporal : fecha_Servidor + new TimeSpan(17, 30, 0);

                strValorTemporal = confg.getValueAppConfig("FECHATKTKPT", "PARAMETRO");
                confg.fechaOp1 = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? fecha_Servidor.AddMinutes(10) : dtValorTemporal;

                strValorTemporal = confg.getValueAppConfig("FECHAKPTTXT", "PARAMETRO");
                confg.fechaOp2 = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? confg.fechaOp1.AddMinutes(5) : dtValorTemporal;

                strValorTemporal = confg.getValueAppConfig("FECHATXTSQL", "PARAMETRO");
                confg.fechaOp3 = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? fecha_Servidor.AddMinutes(60) : dtValorTemporal;

                strValorTemporal = confg.getValueAppConfig("FECHASWFSQL", "PARAMETRO");
                confg.fechaOp4 = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? fecha_Servidor.AddMinutes(70) : dtValorTemporal;


                confg.pathEquation = confg.getValueAppConfig("PATHEQUATION", "PARAMETRO");
                confg.pathSwift = confg.getValueAppConfig("PATHSWIFT", "PARAMETRO");
                confg.pathModels = confg.getValueAppConfig("PATHMODELS", "PARAMETRO");
                confg.pathFtpApp = confg.getValueAppConfig("PATHFTPAPP", "PARAMETRO");
                confg.pathSaldos = confg.getValueAppConfig("PATHSALDOS", "PARAMETRO");

                confg.envioBl = (confg.getValueAppConfig("CHKBANKLINK", "PARAMETRO") == "1");

                intValorTemporal = Int32.Parse(confg.getValueAppConfig("PERIODOBANKLINK", "PARAMETRO"));
                confg.periodoBl = (intValorTemporal < 1 || intValorTemporal > 24) ? 10 : intValorTemporal;

                strValorTemporal = confg.getValueAppConfig("FECHABANKLINK", "PARAMETRO");
                confg.fechaBl = (DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? dtValorTemporal : DateTime.Now.AddHours(confg.periodoBl);

                confg.envio = new bool[17];

                confg.envio[0] = (confg.getValueAppConfig("CHK0", "PARAMETRO") == "1");
                confg.envio[1] = (confg.getValueAppConfig("CHK1", "PARAMETRO") == "1");
                confg.envio[2] = (confg.getValueAppConfig("CHK2", "PARAMETRO") == "1");
                confg.envio[3] = (confg.getValueAppConfig("CHK3", "PARAMETRO") == "1");
                confg.envio[4] = (confg.getValueAppConfig("CHK4", "PARAMETRO") == "1");
                confg.envio[5] = (confg.getValueAppConfig("CHK5", "PARAMETRO") == "1");
                confg.envio[6] = (confg.getValueAppConfig("CHK6", "PARAMETRO") == "1");
                confg.envio[7] = (confg.getValueAppConfig("CHK7", "PARAMETRO") == "1");
                confg.envio[8] = (confg.getValueAppConfig("CHK8", "PARAMETRO") == "1");
                confg.envio[9] = (confg.getValueAppConfig("CHK9", "PARAMETRO") == "1");
                confg.envio[10] = (confg.getValueAppConfig("CHK10", "PARAMETRO") == "1");
                confg.envio[11] = (confg.getValueAppConfig("CHK11", "PARAMETRO") == "1");
                confg.envio[12] = (confg.getValueAppConfig("CHK12", "PARAMETRO") == "1");
                confg.envio[13] = (confg.getValueAppConfig("CHK13", "PARAMETRO") == "1");
                confg.envio[14] = (confg.getValueAppConfig("CHK14", "PARAMETRO") == "1");
                confg.envio[15] = (confg.getValueAppConfig("CHK15", "PARAMETRO") == "1");
                confg.envio[16] = (confg.getValueAppConfig("CHK16", "PARAMETRO") == "1");

                confg.noIntentos = Int32.Parse(confg.getValueAppConfig("INTENTOSKAPITITXT", "PARAMETRO"));

                confg.usr400 = crpt.VerificaClaves(2, confg.getValueAppConfig("Usuario", "AS400"));
                confg.pswd400 = crpt.VerificaClaves(2, confg.getValueAppConfig("Password", "AS400"));
                confg.dsn400 = crpt.VerificaClaves(2, confg.getValueAppConfig("DSN", "AS400"));

            
                RevisaExclusiones();

                gsEnvioXML = confg.getValueAppConfig("ENVIOXML", "PARAMETRO");
                gsRutaEXEXML = confg.getValueAppConfig("RutaExeXML", "PARAMETRO");
                gsMQEscribirXML = confg.getValueAppConfig("MQESCRIBIRXML", "PARAMETRO");
                gsMQLeerXML = confg.getValueAppConfig("MQLEERXML", "PARAMETRO");
                
                gsEnvioMT = confg.getValueAppConfig("ENVIOMT", "PARAMETRO");

                return true;

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);

                return false;
            }
        }


        private string GetCredential(DbConnection cnn, string busqueda)
        {
            string conexion = cnn.ConnectionString;

            int idx = conexion.IndexOf(busqueda) + busqueda.Length;
            int i = 0;
            string usuario = "";

            foreach (char c in conexion)
            {
                if(i >= idx)
                {
                    if (c == ';') break;
                    usuario += c;                  
                }
                i++;
            }

            return usuario;
        }

        private void RevisaExclusiones()
        {
            try
            {
                byte lnAgencia, lnValue, lnBit;

                msSendCtes = "";
                msSendTDs = "";
                msSendDepRet = "";
                msSendDepRetAj = "";
                msSendROPD = "";
                msSendTrasp = "";
                msSendMT103 = "";
                msSendHOLD = "";
                msSendSwift = "";
                msSendTDOver = "";

                lnAgencia = 1;
                

                for(lnBit = 0; lnBit <= (confg.sendLaops.Length - 1); lnBit++)
                {
                    lnValue = byte.Parse(confg.sendLaops.Substring(lnBit, 1));

                    switch (lnBit)
                    {
                        //Reportes MT103
                        case 1:
                            if(lnValue == 1)
                            {
                                if (msSendMT103 == "")
                                {
                                    msSendMT103 = $" and PC.agencia in ({lnAgencia})";
                                }
                                else
                                {
                                    msSendMT103 += $", {lnAgencia}";
                                }
                            }
                            
                        break;
                        //Reportes Swift
                        case 2:
                            if (lnValue == 1)
                            {
                                if (msSendSwift == "")
                                {
                                    msSendSwift = $" and AG.agencia in ({lnAgencia})";
                                }
                                else
                                {
                                    msSendSwift += $", {lnAgencia}";
                                }
                            }
                            break;
                        //Reportes MT202
                        case 3:
                            if (lnValue == 1)
                            {
                                if (msSendMT202 == "")
                                {
                                    msSendMT202 = $" and agencia in ({lnAgencia})";
                                }
                                else
                                {
                                    msSendMT202 += $", {lnAgencia}";
                                }
                            }
                            break;
                        //Clientes
                        case 4:
                            if (lnValue == 1)
                            {
                                if (msSendCtes == "")
                                {
                                    msSendCtes = $" and AG.agencia in ({lnAgencia})";
                                }
                                else
                                {
                                    msSendCtes += $", {lnAgencia}";
                                }
                            }
                            break;
                        //Depositos y Retiros
                        case 5:
                            if (lnValue == 1)
                            {
                                if (msSendDepRet == "")
                                {
                                    msSendDepRet = $" and AG.agencia in ({lnAgencia})";
                                }
                                else
                                {
                                    msSendDepRet += $", {lnAgencia}";
                                }
                            }
                            break;
                        //Depositos y Retiros por Ajuste
                        case 6:
                            if (lnValue == 1)
                            {
                                if (msSendDepRetAj == "")
                                {
                                    msSendDepRetAj = $" and AG.agencia in ({lnAgencia})";
                                }
                                else
                                {
                                    msSendDepRetAj += $", {lnAgencia}";
                                }
                            }
                            break;
                        //Depositos y Retiros TDD
                        case 7:
                            if (lnValue == 1)
                            {
                                if (msSendDepRetTDD == "")
                                {
                                    msSendDepRetTDD = $" and AG.agencia in ({lnAgencia})";
                                }
                                else
                                {
                                    msSendDepRetTDD += $", {lnAgencia}";
                                }
                            }
                            break;
                        //Órdenes de Pago en USD
                        case 8:
                            if (lnValue == 1)
                            {
                                if (msSendROPD == "")
                                {
                                    msSendROPD = $" and AG.agencia in ({lnAgencia})";
                                }
                                else
                                {
                                    msSendROPD += $", {lnAgencia}";
                                }
                            }
                            break;
                        //Traspasos
                        case 9:
                            if (lnValue == 1)
                            {
                                if (msSendTrasp == "")
                                {
                                    msSendTrasp = $" and AG.agencia in ({lnAgencia})";
                                }
                                else
                                {
                                    msSendTrasp += $", {lnAgencia}";
                                }
                            }
                            break;
                        //Time Deposit's
                        case 10:
                            if (lnValue == 1)
                            {
                                if (msSendTDs == "")
                                {
                                    msSendTDs = $" and PC.agencia in ({lnAgencia})";
                                }
                                else
                                {
                                    msSendTDs += $", {lnAgencia}";
                                }
                            }
                            break;
                        //Time Deposit's Overnight
                        case 11:
                            if (lnValue == 1)
                            {
                                if (msSendTDOver == "")
                                {
                                    msSendTDOver = $" and PC.agencia in ({lnAgencia})";
                                }
                                else
                                {
                                    msSendTDOver += $", {lnAgencia}";
                                }
                            }
                            break;
                        //HOLDS
                        case 12:
                            if (lnValue == 1)
                            {
                                if (msSendHOLD == "")
                                {
                                    msSendHOLD = $" and PC.agencia in ({lnAgencia})";
                                }
                                else
                                {
                                    msSendHOLD += $", {lnAgencia}";
                                }
                            }
                            break;
                    }                   
                }

                if(msSendCtes != "")
                {
                    msSendCtes += ") ";
                }
                else
                {
                    msSendCtes = " and AG.agencia not in (1,2,3) ";
                }

                if(msSendTDs != "")
                {
                    msSendTDs += ") ";
                }
                else
                {
                    msSendTDs = " and PC.agencia not in (1,2,3) ";
                }

                if (msSendDepRet != "")
                {
                    msSendDepRet += ") ";
                }
                else
                {
                    msSendDepRet = " and AG.agencia not in (1,2,3) ";
                }

                if (msSendDepRet != "")
                {
                    msSendDepRet += ") ";
                }
                else
                {
                    msSendDepRet = " and AG.agencia not in (1,2,3) ";
                }

                if (msSendDepRetAj != "")
                {
                    msSendDepRetAj += ") ";
                }
                else
                {
                    msSendDepRetAj = " and AG.agencia not in (1,2,3) ";
                }

                if (msSendTrasp != "")
                {
                    msSendTrasp += ") ";
                }
                else
                {
                    msSendTrasp = " and AG.agencia not in (1,2,3) ";
                }

                if (msSendMT103 != "")
                {
                    msSendMT103 += ") ";
                }
                else
                {
                    msSendMT103 = " and PC.agencia not in (1,2,3) ";
                }

                if (msSendSwift != "")
                {
                    msSendSwift += ") ";
                }
                else
                {
                    msSendSwift = " and AG.agencia not in (1,2,3) ";
                }

                if (msSendTDOver != "")
                {
                    msSendTDOver += ") ";
                }
                else
                {
                    msSendTDOver = " and PC.agencia not in (1,2,3) ";
                }

                if (msSendROPD != "")
                {
                    msSendROPD += ") ";
                }
                else
                {
                    msSendROPD = " and AG.agencia not in (1,2,3) ";
                }

                if (msSendMT202 != "")
                {
                    msSendMT202 += ") ";
                }
                else
                {
                    msSendMT202 = " and agencia not in (1,2,3) ";
                }

                if (msSendHOLD != "")
                {
                    msSendHOLD += ") ";
                }
                else
                {
                    msSendHOLD = " and agencia not in (1,2,3) ";
                }
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void ConectarAS400()
        {
            as400 = new ConexionAS400(confg.usr400, confg.pswd400, confg.dsn400);

            //[PRUEBAS] consulta a AS400***************************************************
            DataTable dt = as400.EjecutaSelect("SELECT getdate() FROM TKTLIB.DEIB5406");

            foreach (DataRow row in dt.Rows)
            {
                string algo1 = row[0].ToString();
                string algo2 = row[1].ToString();
            }
            //*****************************************************************************

        }

        private void PararInterface()
        {           
            //2,3,4,9,10
            btnTransferencia.Enabled = false;
            btnCargas.Enabled = false;
            btnPendientes.Enabled = false;
            btnCierreEnv.Enabled = false;
            btnMonitoreo.Enabled = false;
            Log.Escribe("Interfaz Apagada");
            StopTimers();
            EncenderLed(3);
            

        }

        private void StopTimers()
        {
            tmrEnvio.Enabled = false;
        }

        /// <summary>
        /// Enciende el led indicado y apaga los demas
        /// </summary>
        /// <param name="led">1: Verde, 2: Amarillo, 3: Rojo</param>
        private void EncenderLed(int led)
        {
            switch (led)
            {
                case 1:
                    ledVerde.Visible = true;
                    ledAmarillo.Visible = false;
                    ledRojo.Visible = false;                                 
                    break;
                case 2:
                    ledVerde.Visible = false;
                    ledAmarillo.Visible = true;
                    ledRojo.Visible = false;
                    break;
                case 3:
                    ledVerde.Visible = false;
                    ledAmarillo.Visible = false;
                    ledRojo.Visible = true;
                    break;
                default:
                    ledVerde.Visible = false;
                    ledAmarillo.Visible = false;
                    ledRojo.Visible = false;
                    break;
            }
           
        }

        private void ResetTimers()
        {
            if(!booNoChangeParam && !booNoChangeConf)
            {
                tmrEnvio.Enabled = true;
            }          
        }

        private void btnTransferencia_Click(object sender, EventArgs e)
        {
            
        }

     

        private void switchButton1_Click(object sender, EventArgs e)
        {
            try
            {             
                if (switchButton1.Checked)
                {
                    gbEncendido = true;
                }
                else
                {
                    gbEncendido = false;
                }

                EncenderInterfaz(gbEncendido);
                
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void EncenderInterfaz(bool gbEncendido)
        {
            if (gbEncendido)
            {
                gbPasswordOK = false;

                frmSolicPassword frm = new frmSolicPassword(this);
                frm.ShowDialog();


                if (!gbPasswordOK)
                {
                    switchButton1.Checked = false;
                    swith_click = false;
                    return;
                }

                if (ModeloNegocio.Transferencias.LlenarColeccion())
                {
                    if (DespliegaDatos())
                    {
                        if (mbTransStatus)
                        {
                            frm_monitor = new frmMonitoreo(this);
                            tmrLog.Enabled = true;

                            frm_monitor.tmrMonitor.Enabled = false;
                            frm_monitor.Visible = false;
                            frm_monitor.Hide();


                            btnTransferencia.Enabled = true;
                            btnCargas.Enabled = true;
                            btnPendientes.Enabled = true;
                            btnCierreEnv.Enabled = true;
                            btnMonitoreo.Enabled = true;

                            EncenderLed(1);
                            ResetTimers();
                        }
                        else
                        {
                            tsLblServer.Text = "La transferencia de Archivos ya esta cerrada";
                            btnCargas.Enabled = true;

                            EncenderLed(1);
                            ResetTimers();
                        }

                    }
                    else
                    {
                        tsLblServer.Text = "Verifique el servidor y su conexión a la red local o si el sistema ya cerró.";
                    }
                }
            }
            else
            {
                gbPasswordOK = false;

                frmSolicPassword frm = new frmSolicPassword(this);
                frm.ShowDialog();

                frm_monitor.Visible = false;

                if (gbPasswordOK)
                {
                    PararInterface();
                }


            }
        }

        public bool DespliegaDatos()
        {
            DateTime fecha_Servidor = ModeloNegocio.Parametro.FechaServidor();

            Datos.ParametrosEjecucion resultado = ModeloNegocio.Parametro.GetParametrosEjecucion();

            if (resultado != null)
            {
                if (fecha_Servidor.ToString("dd-MM-yyyy") != resultado.Fecha_Sistema.ToString("dd-MM-yyyy"))
                {
                    Log.Escribe("La fecha en parámetros no es la misma que la fecha del servidor!");
                }
                if (fecha_Servidor.ToString("dd-MM-yyyy") != DateTime.Now.ToString("dd-MM-yyyy"))
                {
                    Log.Escribe("La fecha de la PC no es la misma que la fecha del servidor!");
                }
            }
            else
            {
                Log.Escribe("No existen datos en la tabla de PARAMETROS", "Error");
                return false;
            }

            Datos.Parametros to_tregs_fecha = ModeloNegocio.Parametro.GetParametrizacion("TOTREGSFECHA");
            Datos.Parametros trans_status = ModeloNegocio.Parametro.GetParametrizacion("TRANSSTATUS");

            //Si la fecha de transferencia es hoy

            DateTime tregs_fecha = Convert.ToDateTime(DateTime.ParseExact(to_tregs_fecha.valor.Trim(), "MM-dd-yyyy", CultureInfo.InvariantCulture));
            if ((tregs_fecha - resultado.Fecha_Servidor).Days == 0)
            {

                if (trans_status.valor.Trim() == "CLOSED")
                {
                    mbTransStatus = false;
                    Log.Escribe("Transferencia Cerrada!");
                }
                else
                {
                    mbTransStatus = true;
                }
            }
            else
            {
                if (trans_status.valor.Trim() == "CLOSED")
                {
                    //Inicializa los parámetros de ejecucion
                    ModeloNegocio.Parametro.ActualizaParametro("TOTREGSFECHA", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                     ModeloNegocio.Parametro.ActualizaParametro("TOTREGSFECHA", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                     ModeloNegocio.Parametro.ActualizaParametro("TRANSSTATUS", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                     ModeloNegocio.Parametro.ActualizaParametro("TOTREGSMT103", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                     ModeloNegocio.Parametro.ActualizaParametro("TOTREGSMT202", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                     ModeloNegocio.Parametro.ActualizaParametro("TOTREGSSWIFT", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                    ModeloNegocio.Parametro.ActualizaParametro("TOTREGSCTES", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));

                    //DGI (EDS) MANTENIMIENTO
                    ModeloNegocio.Parametro.ActualizaParametro("TOTREGSMANT", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));

                     ModeloNegocio.Parametro.ActualizaParametro("TOTREGSDEPRET", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                     ModeloNegocio.Parametro.ActualizaParametro("TOTREGSHOLDS", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                     ModeloNegocio.Parametro.ActualizaParametro("TOTREGSROPD", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                     ModeloNegocio.Parametro.ActualizaParametro("TOTREGSTRASP", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                     ModeloNegocio.Parametro.ActualizaParametro("TOTREGSTDS", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));

                    //EPH020305                      
                    gsFecOp1 = fecha_Servidor.AddMinutes(10);
                    gsFecOp2 = gsFecOp1.AddMinutes(5);
                    gsFecOp3 = gsFecOp2.AddMinutes(5);
                    gsFecOp4 = gsFecOp3.AddMinutes(5);

                    gsFecResp = Funciones.Fecha("FIN", confg.fechaCierre);
                    gsFecSV1 = Funciones.Fecha("FIN", confg.fechaCierre);
                    gsFecSV2 = Funciones.Fecha("FIN", confg.fechaCierre);
                    confg.fechaCierre = new DateTime(fecha_Servidor.Year, fecha_Servidor.Month, fecha_Servidor.Day) + confg.horaCierre;
                }
            }

            //Lee el numero de operaciones procesadas
            gsTotMT103 = ModeloNegocio.Parametro.GetParametrizacion("TOTREGSMT103");
            gsTotMT202 = ModeloNegocio.Parametro.GetParametrizacion("TOTREGSMT202");
            gsTotMT198 = ModeloNegocio.Parametro.GetParametrizacion("TOTREGSSWIFT");
            gsTotClientes = ModeloNegocio.Parametro.GetParametrizacion("TOTREGSCTES");
            //DGI (EDS) Mantenimiento
            gsTotMantto = ModeloNegocio.Parametro.GetParametrizacion("TOTREGSMANT");

            gsTotDepRet = ModeloNegocio.Parametro.GetParametrizacion("TOTREGSDEPRET");
            gsTotHOLDRet = ModeloNegocio.Parametro.GetParametrizacion("TOTREGSHOLDS");
            gsTotROPUSD = ModeloNegocio.Parametro.GetParametrizacion("TOTREGSROPD");
            gsTotTraspasos = ModeloNegocio.Parametro.GetParametrizacion("TOTREGSTRASP");
            gsTotTDs = ModeloNegocio.Parametro.GetParametrizacion("TOTREGSTDS");

            CargaSalVen.CalcDiaSig();

            VerificaRespaldo();

            return true;


        }

        private static bool VerificaRespaldo()
        {
            gsPaso = ModeloNegocio.Parametro.GetParametro("error_saldos_diarios");

            if (gsPaso != -1)
            {
                gsBackup = ModeloNegocio.Parametro.GetParametro("backup_saldos_diarios");

                if (gsBackup == -1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private void tmrLog_Tick(object sender, EventArgs e)
        {
            string archivo_path = Log.RutaLog + "\\" + Log.Archivo_Actual;
            string texto = "";

            if (File.Exists(archivo_path))
            {
                StreamReader sr = new StreamReader(archivo_path);

                String linea = sr.ReadLine();

                while (linea != null)
                {
                    texto += linea + Environment.NewLine;
                    linea = sr.ReadLine();
                }
                sr.Close();

                txtLog.Text = texto;
                txtLog.SelectionStart = txtLog.Text.Length - 1;
                txtLog.ScrollToCaret();

            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            confg.SetParameterAppSettings("RESET", "0", "APLICACION");
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void switchButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnParams_Click(object sender, EventArgs e)
        {
            frmParametros frm = new frmParametros(this);
            frm.ShowDialog();
        }

        private void Pantalla_Principal_Activated(object sender, EventArgs e)
        {

        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            frmInformacion frm = new frmInformacion(this);
            frm.ShowDialog();
        }
    }
}
