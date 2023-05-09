using MNICript;
using ModeloNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfazEnvios
{
    public partial class frmParametros : Form
    {
        private bool booActivate;
        private bool booNoChangeParam;
        private Pantalla_Principal frmp;
        private bool booNoChangeConf;
        private DataTable dt;

        clsEncripta crpt;

        public frmParametros(Pantalla_Principal frmp)
        {
            InitializeComponent();
            this.frmp = frmp;
        }

        private void frmParametros_Activated(object sender, EventArgs e)
        {
            if(!booActivate)
            {
                Log.Escribe("Password de Confirmación.");
                booNoChangeParam = false;
                SinCambio();
            }

            CargaParametros();

            

        }

        private void CargaParametros()
        {
            //AS400
            txtNombreSna.Text =  frmp.confg.sna620;
            txtBibliotecaEq.Text = frmp.confg.libSwift;
            txtBibliotecaSaldos.Text = frmp.confg.libSaldos;
            txtBibliotecaEnv.Text = frmp.confg.libDefault;

            //MQ Series
            txtQueManager.Text = frmp.confg.mqManager;
            txtQueEnv.Text = frmp.confg.mqEscribir;
            txtQueRecib.Text = frmp.confg.mqLeer;
            txtQueReprt.Text = frmp.confg.mqReporte;

            PrepararGrid();
        }

        private void PrepararGrid()
        {
            dt = new DataTable();
            dt.Clear();

            dt.Columns.Add("Operaciones", typeof(string));
            dt.Columns.Add("Houston", typeof(bool));


            string[] columnas = { "Swift MT103", "Swift MT198", "Swift MT202", "Clientes", "Depósitos/Retiros", "Deps/Rets por Ajuste", "Deps/Rets por TDD", "Órdenes de Pago USD", "Traspasos", "Time Deposit's", "Time Deposit's Overnight", "Holds"};
            int i = 0;

            foreach (string col in columnas)
            {
                DataRow dr = dt.NewRow();
                dr["Operaciones"] = col;
                dr["Houston"] = (frmp.confg.sendLaops[i] == '1');
                dt.Rows.Add(dr);
                i++;
            }

            dtgvInfo.DataSource = dt;
            dtgvInfo.Refresh();

            chkEnvArchivos.Checked = (frmp.confg.sendAS400 == "1");
            chkEnvMsj.Checked = (frmp.confg.slashMT103 == "1");
        }

        private void SinCambio()
        {
            btnGuardar.Enabled = false;
            booNoChangeParam = false;

            if (frmp.gbEncendido) ResetTimers();
        }

        private void HayCambio()
        {
            StopTimers();
            btnGuardar.Enabled = true;
            booNoChangeParam = true;

            if (frmp.gbEncendido) ResetTimers();
        }

        private void StopTimers()
        {
            frmp.tmrEnvio.Enabled = false;
        }

        private void ResetTimers()
        {
            if(!booNoChangeParam && !booNoChangeConf)
            {
                frmp.tmrEnvio.Enabled = true;
            }
        }

        private void frmParametros_Load(object sender, EventArgs e)
        {
            crpt = new clsEncripta();

            booNoChangeParam = false;

            List<string> ambientes = new List<string>();
            ambientes.Add("Produccion");
            ambientes.Add("Desarrollo");
            cmbBic.DataSource = ambientes;
        }

        private void cmbBic_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbBic.SelectedValue.ToString();

            if (selected == "Produccion")
            {
                txtBicOrigen.Text = ModeloNegocio.Parametro.GetParametrizacion("BCOBENEFR1").valor;
                txtOrigenDestino.Text = ModeloNegocio.Parametro.GetParametrizacion("BCOBENEFD1").valor;
            }
            else if(selected == "Desarrollo")
            {
                txtBicOrigen.Text = ModeloNegocio.Parametro.GetParametrizacion("BCOBENEFR1P").valor;
                txtOrigenDestino.Text = ModeloNegocio.Parametro.GetParametrizacion("BCOBENEFD1P").valor;
            }    
        }

        private void btnCambiar_Click(object sender, EventArgs e)
        {
            if(txtPswdAct.Text != frmp.confg.gsAppPassword)
            {
                MessageBox.Show("El password actual no es el correcto", "Error cambio de password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPswdAct.Text = "";
                txtNwPswd.Text = "";
                txtConfirmPswd.Text = "";
            }
            else if (txtNwPswd.Text == "")
            {
                MessageBox.Show("El nuevo Password no puede estar enl blanco", "Error cambio de password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPswdAct.Text = "";
                txtNwPswd.Text = "";
                txtConfirmPswd.Text = "";
            }
            else if (txtNwPswd.Text != txtConfirmPswd.Text)
            {
                MessageBox.Show("No coincide el password y la confirmacion del password", "Error cambio de password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPswdAct.Text = "";
                txtNwPswd.Text = "";
                txtConfirmPswd.Text = "";
            }
            else
            {
                frmp.confg.gsAppPassword = crpt.VerificaClaves(1, txtNwPswd.Text);
                frmp.confg.SetParameterAppSettings("APPPWD", frmp.confg.gsAppPassword, "PARAMETROS");
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            bool lbReset = false;
            Log.Escribe("Actualizacion de parametros...");

            //Busca cambios que requieran un reinicio de la aplicacion
            lbReset = (frmp.confg.sna620 != txtNombreSna.Text);
            lbReset = (frmp.confg.libSwift != txtBibliotecaEq.Text);
            lbReset = (frmp.confg.libSaldos != txtBibliotecaSaldos.Text);
            lbReset = (frmp.confg.libDefault != txtBibliotecaEnv.Text);

            AsignaValores();

            UpdateRegistro();

            Log.Escribe("Los cambios fueron actualizados con éxito. Parámetros");

            if(lbReset)
            {
                Log.Escribe("Es necesario reinciiar la interfaz para que los cambios se vean reflekadpos");
            }

            SinCambio();


        }

        private void UpdateRegistro()
        {
            //AS400
            frmp.confg.SetParameterAppSettings("SNA620", crpt.VerificaClaves(1, frmp.confg.sna620), "PARAMETROS");
            frmp.confg.SetParameterAppSettings("LIBSWIFT", crpt.VerificaClaves(1, frmp.confg.libSwift), "PARAMETROS");
            frmp.confg.SetParameterAppSettings("LIBSALDOS", crpt.VerificaClaves(1, frmp.confg.libSaldos), "PARAMETROS");
            frmp.confg.SetParameterAppSettings("LIBDEFAULT", crpt.VerificaClaves(1, frmp.confg.libDefault), "PARAMETROS");

            //Para mapeo y envio de archivos
            frmp.confg.SetParameterAppSettings("MQMANAGER", crpt.VerificaClaves(1, frmp.confg.mqManager), "PARAMETROS");
            frmp.confg.SetParameterAppSettings("MQESCRIBIR", crpt.VerificaClaves(1, frmp.confg.mqEscribir), "PARAMETROS");
            frmp.confg.SetParameterAppSettings("MQLEER", crpt.VerificaClaves(1, frmp.confg.mqLeer), "PARAMETROS");
            frmp.confg.SetParameterAppSettings("MQREPORTE", crpt.VerificaClaves(1, frmp.confg.mqReporte), "PARAMETROS");

            //Generacion de Informacion
            frmp.confg.SetParameterAppSettings("SENDLAOPS", crpt.VerificaClaves(1, frmp.confg.sendLaops), "PARAMETROS");
            frmp.confg.SetParameterAppSettings("SENDAS400", crpt.VerificaClaves(1, frmp.confg.sendAS400), "PARAMETROS");
            frmp.confg.SetParameterAppSettings("SLASHMT103", crpt.VerificaClaves(1, frmp.confg.slashMT103), "PARAMETROS");

            //Password de configuración de parámetros
            frmp.confg.SetParameterAppSettings("APPPWD", crpt.VerificaClaves(1, frmp.confg.mqReporte), "PARAMETROS");
        }

        private void AsignaValores()
        {
            //Ambiente
            frmp.confg.ambiente = cmbBic.SelectedValue.ToString();

            //AS400
            frmp.confg.sna620 = txtNombreSna.Text;
            frmp.confg.libSwift = txtBibliotecaEq.Text;
            frmp.confg.libSaldos = txtBibliotecaSaldos.Text;
            frmp.confg.libDefault = txtBibliotecaEnv.Text;

            //MQ-Series
            frmp.confg.mqManager = txtQueManager.Text;
            frmp.confg.mqEscribir = txtQueEnv.Text;
            frmp.confg.mqLeer = txtQueRecib.Text;
            frmp.confg.mqReporte = txtQueReprt.Text;


            //Generacion de Informacion 
            foreach (DataGridViewRow dr in dtgvInfo.Rows)
            {

                bool check = ((bool)dr.Cells["Houston"].Value == true);

                frmp.confg.sendLaops += (check)? "1":"0";
            }

            frmp.confg.sendAS400 = (chkEnvArchivos.Checked)? "1": "0";
            frmp.confg.slashMT103 = (chkEnvMsj.Checked)? "1": "0";
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            bool blnConectado, blnVerificarEscribir, blnVerificarLeer;
            string strError;

            Log.Escribe("Test de conexion MQSeries");


        }
    }
}
