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

                ConectarAS400();

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

            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }
        }

        private void ConectarAS400()
        {
            string usuario = crpt.VerificaClaves(2, Configuracion.getValueAppConfig("Usuario", "AS400"));
            string password = crpt.VerificaClaves(2, Configuracion.getValueAppConfig("Password", "AS400"));
            string dsn = crpt.VerificaClaves(2, Configuracion.getValueAppConfig("DSN", "AS400"));

            as400 = new ConexionAS400(usuario, password, dsn);

            //[PRUEBAS] consulta a AS400***************************************************
            DataTable dt = as400.EjecutaSelect("SELECT * FROM DATAMART.CEN01F");

            foreach (DataRow row in dt.Rows)
            {
                string algo = row[6].ToString();
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
