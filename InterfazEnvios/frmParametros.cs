﻿using ModeloNegocio;
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
            txtNombreSna.Text = frmp.confg.sna620;
            txtBibliotecaEq.Text = frmp.confg.libSwift;
            txtBibliotecaSaldos.Text = frmp.confg.libSaldos;
            txtBibliotecaEnv.Text = frmp.confg.libDefault;

            //MQ Series
            txtQueManager.Text = frmp.confg.mqManager;
            txtQueEnv.Text = frmp.confg.mqEscribir;
            txtQueRecib.Text = frmp.confg.mqLeer;
            txtQueReprt.Text = frmp.confg.mqReporte;

            
        }

        private void SinCambio()
        {
            btnGuardar.Enabled = false;
            booNoChangeParam = false;

            if (frmp.gbEncendido) ResetTimers();
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
            booNoChangeParam = false;

            List<string> ambientes = new List<string>();
            ambientes.Add("Produccion");
            ambientes.Add("Desarrollo");
            cmbBic.DataSource = ambientes;
        }
    }
}