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
    public partial class frmMonitoreo : Form
    {
        Pantalla_Principal frmp;
        public frmMonitoreo(Pantalla_Principal frmp)
        {
            this.frmp = frmp;

            InitializeComponent();
        }
    }
}
