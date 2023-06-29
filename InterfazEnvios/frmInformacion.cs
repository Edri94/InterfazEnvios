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
    public partial class frmInformacion : Form
    {
        private Pantalla_Principal frmp;
        private DataTable dtRutas, dtArchivos;
        public frmInformacion(Pantalla_Principal frmp)
        {
            this.frmp = frmp;
       
            InitializeComponent();
        }

        private void frmInformacion_Load(object sender, EventArgs e)
        {
            CargaInformacionList();
        }

       

        private void CargaInformacionList()
        {

            //***Rutas
            dtRutas = new DataTable();
            dtRutas.Columns.Add("Descripcion", typeof(string));
            dtRutas.Columns.Add("Ruta", typeof(string));

            DataRow drr1 = dtRutas.NewRow();
            drr1["Descripcion"] = "Datos (Equation)";
            drr1["Ruta"] = frmp.confg.pathEquation;

            DataRow drr2 = dtRutas.NewRow();
            drr2["Descripcion"] = "Datos (Swift)";
            drr2["Ruta"] = frmp.confg.pathSwift;

            DataRow drr3 = dtRutas.NewRow();
            drr3["Descripcion"] = "Archivos FDF";
            drr3["Ruta"] = frmp.confg.pathModels;

            DataRow drr4 = dtRutas.NewRow();
            drr4["Descripcion"] = "ClientAccess";
            drr4["Ruta"] = frmp.confg.pathFtpApp;

            DataRow drr5 = dtRutas.NewRow();
            drr5["Descripcion"] = "Datos (Saldos)";
            drr5["Ruta"] = frmp.confg.pathSaldos;

            dtRutas.Rows.Add(drr1);
            dtRutas.Rows.Add(drr2);
            dtRutas.Rows.Add(drr3);
            dtRutas.Rows.Add(drr4);
            dtRutas.Rows.Add(drr5);

           
            dtgRutas.DataSource = dtRutas;
            dtgRutas.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgRutas.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgRutas.Refresh();

            //***Archivos
            dtArchivos = new DataTable();
            dtArchivos.Columns.Add("Descripcion", typeof(string));
            dtArchivos.Columns.Add("Archivo", typeof(string));

            Datos.ParametrosEjecucion archivos_enviados = ModeloNegocio.Parametro.GetParametrosEjecucion();

            DataRow dra1 = dtArchivos.NewRow();
            dra1["Descripcion"] = "Reportes MT103 (Archivos TRAN)";
            dra1["Archivo"] = archivos_enviados.MT103;

            DataRow dra2 = dtArchivos.NewRow();
            dra2["Descripcion"] = "Reportes Swift (Archivos SWAG)";
            dra2["Archivo"] = archivos_enviados.Swift;

            DataRow dra3 = dtArchivos.NewRow();
            dra3["Descripcion"] = "Clientes (Archivos CTIB)";
            dra3["Archivo"] = archivos_enviados.CTIB;

            DataRow dra4 = dtArchivos.NewRow();
            dra4["Descripcion"] = "Depósitos y Retiros (Archivos DEIB)";
            dra4["Archivo"] = archivos_enviados.DRT;

            DataRow dra5 = dtArchivos.NewRow();
            dra5["Descripcion"] = "Time Deposits (Archivo TDIB)";
            dra5["Archivo"] = archivos_enviados.TDs;

            DataRow dra6 = dtArchivos.NewRow();
            dra6["Descripcion"] = "Reportes MT202 (Archivo F202)";
            dra6["Archivo"] = archivos_enviados.MT202;

            dtArchivos.Rows.Add(dra1);
            dtArchivos.Rows.Add(dra2);
            dtArchivos.Rows.Add(dra3);
            dtArchivos.Rows.Add(dra4);
            dtArchivos.Rows.Add(dra5);
            dtArchivos.Rows.Add(dra6);

            dtgArchivos.DataSource = dtArchivos;
            dtgArchivos.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgArchivos.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgArchivos.Refresh();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
