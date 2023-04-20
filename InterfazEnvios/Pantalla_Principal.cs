using Datos;
using MNICript;
using ModeloNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfazEnvios
{
    public partial class Pantalla_Principal : Form
    {
        ConexionAS400 as400;
        clsEncripta crpt;
        Configuracion confg;

        bool mbTransStatus;

        public Pantalla_Principal()
        {
            InitializeComponent();

            crpt = new clsEncripta();
        }

        private void Pantalla_Principal_Resize(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Maximized)
            {
                pnlContainer.Width = (this.Width - pnlMenu.Width);
                pnlContainer.Height = (this.Height - pnlNavBar.Height);

                pnlMenu.Height = (this.Height - pnlNavBar.Height);

                pnlNavBar.Width = (this.Width);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Pantalla_Principal_Load(object sender, EventArgs e)
        {
            try
            {
                //*[PRUEBAS] consulta con EF***************************************************
                List<Datos.USUARIO> usuarios = ModeloNegocio.Usuario.Get();


                //***************************************************************************

                //*[PRUEBAS] Funcionamiento ****************************************************************
                Datos.Parametros parametros = ModeloNegocio.Parametro.GetParamnetro("TRANSSTATUS");

                //***************************************************************************

                mbTransStatus = true;

                //Inicializar las variables
                InicializaVariables();



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void InicializaVariables()
        {
            try
            {
                int lnLongitud;
                int intValorTemporal;
                string strValorTemporal;
                DateTime dtValorTemporal;
                TimeSpan hrTemporal;

                confg = new Configuracion();

                confg.gsAppPassword = Configuracion.getValueAppConfig("APPPWD", "PARAMETRO");
                confg.ambiente = Configuracion.getValueAppConfig("AMBIENTE", "PARAMETRO");
                confg.pathFtpApp = Configuracion.getValueAppConfig("PATHFTPAPP", "PARAMETRO");
                confg.sna620 = Configuracion.getValueAppConfig("SNA620", "PARAMETRO");
                confg.libSwift = Configuracion.getValueAppConfig("LIBSWIFT", "PARAMETRO");
                confg.libSaldos = Configuracion.getValueAppConfig("LIBSALDOS", "PARAMETRO");
                confg.libDefault = Configuracion.getValueAppConfig("LIBDEFAULT", "PARAMETRO");
                confg.sendLaops = Configuracion.getValueAppConfig("SENDLAOPS", "PARAMETRO");
                
                if(confg.sendLaops.Length < 12)
                {
                    lnLongitud = 12 - (confg.sendLaops.Length);
                    confg.sendLaops += new string('1', lnLongitud); 
                }

                confg.sendGcOps = Configuracion.getValueAppConfig("SENDGCOPS", "PARAMETRO");

                if (confg.sendGcOps.Length < 12)
                {
                    lnLongitud = 12 - (confg.sendGcOps.Length);
                    confg.sendGcOps += new string('1', lnLongitud);
                }

                confg.sendAS400 = Configuracion.getValueAppConfig("SENDAS400", "PARAMETRO");
                confg.slashMT103 = Configuracion.getValueAppConfig("SLASHMT103", "PARAMETRO");
                
                confg.chkEq = (Configuracion.getValueAppConfig("CHKEQ", "PARAMETRO") == "1");
                confg.chkSw = (Configuracion.getValueAppConfig("CHKSW", "PARAMETRO") == "1");
                confg.chkEnvEqt = (Configuracion.getValueAppConfig("CHKENVEQT", "PARAMETRO") == "1");
                confg.chkEnvSwf = (Configuracion.getValueAppConfig("CHKENVSWF", "PARAMETRO") == "1");
                confg.chkClose = (Configuracion.getValueAppConfig("CHKCLOSE", "PARAMETRO") == "1");
                confg.chkSaldosHc = (Configuracion.getValueAppConfig("CHKSALDOSHO", "PARAMETRO") == "1");
                confg.chkSaldosGc = (Configuracion.getValueAppConfig("CHKSALDOSGC", "PARAMETRO") == "1");
                confg.chkTdsHo = (Configuracion.getValueAppConfig("CHKTDSHO", "PARAMETRO") == "1");
                confg.chkTdsGc = (Configuracion.getValueAppConfig("CHKTDSGC", "PARAMETRO") == "1");
                confg.chkOverGc = (Configuracion.getValueAppConfig("CHKOVERGC", "PARAMETRO") == "1");

                intValorTemporal = Int32.Parse(Configuracion.getValueAppConfig("PERIODO0", "PARAMETRO"));
                confg.periodo0 = (intValorTemporal < 2 || intValorTemporal > 30) ? 10 : intValorTemporal;

                intValorTemporal = Int32.Parse(Configuracion.getValueAppConfig("PERIODO1", "PARAMETRO"));
                confg.periodo1 = (intValorTemporal < 2 || intValorTemporal > 30) ? 10 : intValorTemporal;

                intValorTemporal = Int32.Parse(Configuracion.getValueAppConfig("PERIODO2", "PARAMETRO"));
                confg.periodo2 = (intValorTemporal < 2 || intValorTemporal > 30) ? 10 : intValorTemporal;

                intValorTemporal = Int32.Parse(Configuracion.getValueAppConfig("PERIODO3", "PARAMETRO"));
                confg.periodo3 = (intValorTemporal < 2 || intValorTemporal > 30) ? 10 : intValorTemporal;
   
                intValorTemporal = Int32.Parse(Configuracion.getValueAppConfig("PERIODORESP", "PARAMETRO"));
                confg.periodoResp = (intValorTemporal < 1 || intValorTemporal > 24) ? 10 : intValorTemporal;

                intValorTemporal = Int32.Parse(Configuracion.getValueAppConfig("PERIODOSALDO0", "PARAMETRO"));
                confg.periodoSaldo0 = (intValorTemporal < 1 || intValorTemporal > 24) ? 10 : intValorTemporal;

                intValorTemporal = Int32.Parse(Configuracion.getValueAppConfig("PERIODOSALDO1", "PARAMETRO"));
                confg.periodoSaldo1 = (intValorTemporal < 1 || intValorTemporal > 24) ? 10 : intValorTemporal;

                strValorTemporal =  Configuracion.getValueAppConfig("FECHACIERRE", "PARAMETRO");
                confg.fechaCierre = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ?ModeloNegocio.Parametro.FechaServidor(): dtValorTemporal;

                strValorTemporal = Configuracion.getValueAppConfig("RESPALDO", "PARAMETRO");
                confg.fechaRespaldo = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? Funciones.Fecha("FIN", confg.fechaCierre) : dtValorTemporal;

                strValorTemporal = Configuracion.getValueAppConfig("SALDOFASE1", "PARAMETRO");
                confg.fechaSV1 = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? Funciones.Fecha("FIN", confg.fechaCierre) : dtValorTemporal;

                strValorTemporal = Configuracion.getValueAppConfig("SALDOFASE2", "PARAMETRO");
                confg.fechaSV2 = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? Funciones.Fecha("FIN", confg.fechaCierre) : dtValorTemporal;

                if(mbTransStatus)
                {
                    strValorTemporal = Configuracion.getValueAppConfig("INTERCLOSE", "PARAMETRO");
                    confg.horaCierre = (TimeSpan.TryParse(strValorTemporal, out hrTemporal)) ? hrTemporal : new TimeSpan(23, 0, 0); 
                }

                confg.fechaCierre = Parametro.FechaServidor() + confg.horaCierre;

                //Hora de cierre default cuando la interfaz se carga de forma automática
                strValorTemporal = Configuracion.getValueAppConfig("INTERCLOSEAUTO", "PARAMETRO");
                confg.horaCierreAuto = (TimeSpan.TryParse(strValorTemporal, out hrTemporal)) ? Parametro.FechaServidor()  + hrTemporal : Parametro.FechaServidor() + new TimeSpan(17, 30, 0);

                strValorTemporal = Configuracion.getValueAppConfig("FECHATKTKPT", "PARAMETRO");
                confg.fechaOp1 = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? Parametro.FechaServidor().AddMinutes(10) : dtValorTemporal;

                strValorTemporal = Configuracion.getValueAppConfig("FECHAKPTTXT", "PARAMETRO");
                confg.fechaOp2 = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? confg.fechaOp1.AddMinutes(5) : dtValorTemporal;

                strValorTemporal = Configuracion.getValueAppConfig("FECHATXTSQL", "PARAMETRO");
                confg.fechaOp3 = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? Parametro.FechaServidor().AddMinutes(60) : dtValorTemporal;

                strValorTemporal = Configuracion.getValueAppConfig("FECHASWFSQL", "PARAMETRO");
                confg.fechaOp4 = (!DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? Parametro.FechaServidor().AddMinutes(70) : dtValorTemporal;


                confg.pathEquation = Configuracion.getValueAppConfig("PATHEQUATION", "PARAMETRO");
                confg.pathSwift = Configuracion.getValueAppConfig("PATHSWIFT", "PARAMETRO");
                confg.pathModels = Configuracion.getValueAppConfig("PATHMODELS", "PARAMETRO");
                confg.pathFtpApp = Configuracion.getValueAppConfig("PATHFTPAPP", "PARAMETRO");
                confg.pathSaldos = Configuracion.getValueAppConfig("PATHSALDOS", "PARAMETRO");

                confg.envioBl = (Configuracion.getValueAppConfig("CHKBANKLINK", "PARAMETRO") == "1");

                intValorTemporal = Int32.Parse(Configuracion.getValueAppConfig("PERIODOBANKLINK", "PARAMETRO"));
                confg.periodoBl = (intValorTemporal < 1 || intValorTemporal > 24) ? 10 : intValorTemporal;

                strValorTemporal = Configuracion.getValueAppConfig("FECHABANKLINK", "PARAMETRO");
                confg.fechaBl = (DateTime.TryParse(strValorTemporal, out dtValorTemporal)) ? dtValorTemporal : DateTime.Now.AddHours(confg.periodoBl);

                confg.envio = new bool[7];

                confg.envio[0] = (Configuracion.getValueAppConfig("CHK0", "PARAMETRO") == "1");
                confg.envio[1] = (Configuracion.getValueAppConfig("CHK1", "PARAMETRO") == "1");
                confg.envio[2] = (Configuracion.getValueAppConfig("CHK2", "PARAMETRO") == "1");
                confg.envio[3] = (Configuracion.getValueAppConfig("CHK3", "PARAMETRO") == "1");
                confg.envio[4] = (Configuracion.getValueAppConfig("CHK4", "PARAMETRO") == "1");
                confg.envio[5] = (Configuracion.getValueAppConfig("CHK5", "PARAMETRO") == "1");
                confg.envio[6] = (Configuracion.getValueAppConfig("CHK6", "PARAMETRO") == "1");
                confg.envio[7] = (Configuracion.getValueAppConfig("CHK7", "PARAMETRO") == "1");
                confg.envio[8] = (Configuracion.getValueAppConfig("CHK8", "PARAMETRO") == "1");
                confg.envio[9] = (Configuracion.getValueAppConfig("CHK9", "PARAMETRO") == "1");
                confg.envio[10] = (Configuracion.getValueAppConfig("CHK10", "PARAMETRO") == "1");
                confg.envio[11] = (Configuracion.getValueAppConfig("CHK11", "PARAMETRO") == "1");
                confg.envio[12] = (Configuracion.getValueAppConfig("CHK12", "PARAMETRO") == "1");
                confg.envio[13] = (Configuracion.getValueAppConfig("CHK13", "PARAMETRO") == "1");
                confg.envio[14] = (Configuracion.getValueAppConfig("CHK14", "PARAMETRO") == "1");
                confg.envio[15] = (Configuracion.getValueAppConfig("CHK15", "PARAMETRO") == "1");
                confg.envio[16] = (Configuracion.getValueAppConfig("CHK16", "PARAMETRO") == "1");

                confg.noIntentos = Int32.Parse(Configuracion.getValueAppConfig("INTENTOSKAPITITXT", "PARAMETRO"));

                RevisaExclusiones();


            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void RevisaExclusiones()
        {
            throw new NotImplementedException();
        }

        private void ConectarAS400()
        {
            string usuario = crpt.VerificaClaves(2, Configuracion.getValueAppConfig("Usuario", "AS400"));
            string password = crpt.VerificaClaves(2, Configuracion.getValueAppConfig("Password", "AS400"));
            string dsn = crpt.VerificaClaves(2, Configuracion.getValueAppConfig("DSN", "AS400"));

            as400 = new ConexionAS400(usuario, password, dsn);

            //[PRUEBAS] consulta a AS400***************************************************
            DataTable dt = as400.EjecutaSelect("SELECT * FROM TKTLIB.DEIB5406");

            foreach (DataRow row in dt.Rows)
            {
                string algo1 = row[0].ToString();
                string algo2 = row[1].ToString();
            }
            //*****************************************************************************

        }

        private void switchButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(switchButton1.Checked)
            {

            }
            else
            {

            }
        }

        private void lblEncender_Click(object sender, EventArgs e)
        {

        }
    }
}
