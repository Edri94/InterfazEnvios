using Datos;
using IBM.WMQ;
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
            //SQL
            txtNombreServidor.Text = frmp.confg.dataSource;
            txtLoginUsr.Text = frmp.confg.usrSql;
            txtPswdUsr.Text = frmp.confg.usrSql;

            List<Datos.DataSourceCmb> ambientes = new List<Datos.DataSourceCmb>();
            ambientes.Add(new DataSourceCmb { Display = "Produccion", Valor = "150.100.234.145\\INSSQL18" });
            ambientes.Add(new DataSourceCmb { Display = "Desarrollo", Valor = "100.234.142\\INSSQL002" });
            cmbBic.DataSource = ambientes;
            cmbBic.DisplayMember = "Display";
            cmbBic.ValueMember = "Valor";

            cmbBic.SelectedItem = ambientes.Where(w => w.Display == frmp.confg.ambiente);

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
            try
            {
                bool blnVerificarEscribir = false, blnVerificarLeer = false;


                Log.Escribe("Test de conexion MQSeries");

                if (frmp.gbMQConectado) 
                {
                    if (txtQueManager.Text != frmp.confg.mqManager)
                    {
                        frmp.confg.mqManager = txtQueManager.Text;


                        if (MqSeries.PruebaConexion(frmp.confg.mqManager))
                        {
                            frmp.gbMQConectado = true;
                        }
                        else
                        {
                            frmp.gbMQConectado = false;
                        }
                    }

                    if(frmp.gbMQConectado)
                    {                       
                        if (MqSeries.MQVerificar(frmp.confg.mqManager, txtQueEnv.Text))
                        {
                            blnVerificarLeer = true;
                        }
                        else
                        {
                            blnVerificarLeer = false;
                            Log.Escribe($"Queue: {txtQueEnv.Text}");
                        }
                        if (MqSeries.MQVerificar(frmp.confg.mqManager, txtQueRecib.Text))
                        {
                            blnVerificarLeer = true;
                        }
                        else
                        {
                            blnVerificarLeer = false;
                            Log.Escribe($"Queue: {txtQueRecib.Text}");
                        }

                    }
                }
                else
                {
                    frmp.confg.mqManager = txtQueManager.Text;

                    if (MqSeries.PruebaConexion(frmp.confg.mqManager))
                    {
                        frmp.gbMQConectado = true;

                        if(MqSeries.MQVerificar(frmp.confg.mqManager, txtQueEnv.Text))
                        {
                            blnVerificarEscribir = true;
                        }
                        else
                        {
                            blnVerificarEscribir = false;
                            Log.Escribe($"Queue: {txtQueEnv.Text}");
                        }

                        if (MqSeries.MQVerificar(frmp.confg.mqManager, txtQueRecib.Text))
                        {
                            blnVerificarEscribir = true;
                        }
                        else
                        {
                            blnVerificarEscribir = false;
                            Log.Escribe($"Queue: {txtQueRecib.Text}");
                        }

                    }
                    else
                    {
                        frmp.gbMQConectado = false;
                    }
                }

                if(!blnVerificarEscribir && !blnVerificarLeer)
                {
                    Log.Escribe("Prueba  MQ No satisfactoria");
                    MessageBox.Show("Prueba  MQ No satisfactoria");
                }
                else
                {
                    Log.Escribe("Prueba MQ Satisfactoria");
                    MessageBox.Show("Prueba MQ Satisfactoria");
                }


            }
            catch(MQException mqex)
            {
                Log.Escribe(mqex);
                MessageBox.Show("Error " + mqex.ReasonCode + " , " + mqex.Message);
            }
            catch (Exception ex)
            {
                Log.Escribe("Fallo conexion a MQ");
                Log.Escribe(ex);
            }
            
        }

        private void chkEnvArchivos_Click(object sender, EventArgs e)
        {
            HayCambio();
        }

        private void chkEnvMsj_Click(object sender, EventArgs e)
        {
            HayCambio();
        }

        private void frmParametros_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!booNoChangeParam)
            {
                booActivate = false;
                booNoChangeParam = false;           
            }
            else
            {
                DialogResult dg = MessageBox.Show("¿Desea Salir sin Guardar los Cambios?", "Cambios Pendientes", MessageBoxButtons.YesNo);

                if(dg == DialogResult.Yes)
                {
                    booActivate = false;
                    booNoChangeParam = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
           
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            btnTest.Visible = false;
            btnCambiar.Visible = false;
            btnGuardar.Visible = true;

            switch (e.TabPage.Name)
            {
                case "tbTicket":
                break;

                case "tbAs400":
                break;

                case "tbMqSeries":
                    btnTest.Visible = true;
                break;

                case "tbInfo":
                break;

                case "tbPassword":
                    btnGuardar.Visible = false;
                    btnCambiar.Visible = true;
                break;
            }
        }

        private void txtQueManager_TextChanged(object sender, EventArgs e)
        {
            HayCambio();
        }

        private void txtQueEnv_TextChanged(object sender, EventArgs e)
        {
            HayCambio();
        }

        private void txtQueRecib_TextChanged(object sender, EventArgs e)
        {
            HayCambio();
        }

        private void txtQueReprt_TextChanged(object sender, EventArgs e)
        {
            HayCambio();
        }

        private void txtPswdAct_Leave(object sender, EventArgs e)
        {
            if(frmp.confg.gsAppPassword != txtPswdAct.Text)
            {
                MessageBox.Show("El password actual no es correcto");
                txtPswdAct.Enabled = false;
                txtPswdAct.Focus();
            }
        }

        private void cmbBic_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Datos.DataSourceCmb selected = (Datos.DataSourceCmb)cmbBic.SelectedItem;

            if (selected.Display == "Produccion")
            {
                txtBicOrigen.Text = ModeloNegocio.Parametro.GetParametrizacion("BCOBENEFR1").valor;
                txtOrigenDestino.Text = ModeloNegocio.Parametro.GetParametrizacion("BCOBENEFD1").valor;

                txtNombreServidor.Text = selected.Valor.ToString();
            }
            else if (selected.Display == "Desarrollo")
            {
                txtBicOrigen.Text = ModeloNegocio.Parametro.GetParametrizacion("BCOBENEFR1P").valor;
                txtOrigenDestino.Text = ModeloNegocio.Parametro.GetParametrizacion("BCOBENEFD1P").valor;

                txtNombreServidor.Text = selected.Valor.ToString();
            }
        }
    }
}
