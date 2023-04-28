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
    public partial class frmSolicPassword : Form
    {
        Pantalla_Principal frmp;
        public frmSolicPassword(Pantalla_Principal frmp)
        {
            InitializeComponent();

            this.frmp = frmp;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            frmp.gbPasswordOK = false;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(frmp.confg.gsAppPassword == txtPswd.Text)
            {
                frmp.gbPasswordOK = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Password Incorrecto, Por Favor Verifíquelo e Intente de Nuevo. Aviso", "Error Credenciales", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPswd.Text = "";
                frmp.gbPasswordOK = false;
            }
        }

        private void frmSolicPassword_Load(object sender, EventArgs e)
        {

        }
    }
}
