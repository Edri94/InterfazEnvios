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
    public partial class Pantalla_Principal : Form
    {
        public Pantalla_Principal()
        {
            InitializeComponent();
        }

        private void Pantalla_Principal_Resize(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Maximized)
            {
                //MessageBox.Show($"Resolucion pantalla es: {this.Width} x {this.Height}");
                pnlContainer.Width = (this.Width - pnlMenu.Width);
                pnlContainer.Height = (this.Height - pnlNavBar.Height);

                pnlMenu.Height = (this.Height - pnlNavBar.Height);

                pnlNavBar.Width = (this.Width);
            }

            //Metodo de prueba
            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Close();

        
        }

        private void Pantalla_Principal_Load(object sender, EventArgs e)
        {
            List<Datos.USUARIO> usuarios = ModeloNegocio.Usuario.Get();
        }
    }
}
