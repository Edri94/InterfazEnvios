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
    public partial class frmMonitoreo : Form
    {
        private int LnSeleccion;

        // Variable para la estructura anterior
        private Datos.NotifyConData sysTray;


        // Constantes
        private const long NOTIFYICON_VERSION = 3;
        private const long NOTIFYICON_OLDVERSION = 0;

        private const long NIM_ADD = 0x0;
        private const long NIM_MODIFY = 0x1;
        private const long NIM_DELETE = 0x2;

        private const long NIM_SETFOCUS = 0x3;
        private const long NIM_SETVERSION = 0x4;

        private const long NIF_MESSAGE = 0x1;
        private const long NIF_ICON = 0x2;
        private const long NIF_TIP = 0x4;

        private const long NIF_STATE = 0x8;
        private const long NIF_INFO = 0x10;

        private const long NIS_HIDDEN = 0x1;
        private const long NIS_SHAREDICON = 0x2;

        private const long NIIF_NONE = 0x0;
        private const long NIIF_WARNING = 0x2;
        private const long NIIF_ERROR = 0x3;
        private const long NIIF_INFO = 0x1;
        private const long NIIF_GUID = 0x4;

        private const long WM_MOUSEMOVE = 0x200;
        private const long WM_LBUTTONDOWN = 0x201;
        private const long WM_LBUTTONUP = 0x202;
        private const long WM_LBUTTONDBLCLK = 0x203;
        private const long WM_RBUTTONDOWN = 0x204;
        private const long WM_RBUTTONUP = 0x205;
        private const long WM_RBUTTONDBLCLK = 0x206;

        public bool Bitacora_IE;


        Pantalla_Principal frmp;

        DataTable dtMarco;
        DataTable dtEnviarRecibir;

        public frmMonitoreo(Pantalla_Principal frmp)
        {
            this.frmp = frmp;

            InitializeComponent();
        }

        private void frmMonitoreo_Load(object sender, EventArgs e)
        {
            try
            {
                switch (LnSeleccion)
                {
                    case 0:
                        ModeloNegocio.Transferencia.GeneraTRAN();
                        break;

                    case 1:
                        ModeloNegocio.Transferencia.GeneraSWAG();
                        break;

                    case 2:
                        ModeloNegocio.Transferencia.GeneraMT202();
                        break;

                    case 3:
                        ModeloNegocio.Transferencia.GeneraCTIB();
                        break;

                    case 4:
                        ModeloNegocio.Transferencia.GeneraCTIB();
                        break;

                    case 5:
                    case 7:
                        ModeloNegocio.Transferencia.GeneraDEIB(1);
                        break;

                    case 6:
                        ModeloNegocio.Transferencia.GeneraDEIB(5);
                        break;

                    case 8:
                        ModeloNegocio.Transferencia.GeneraDEIB(5);
                        break;

                    case 9:
                        ModeloNegocio.Transferencia.GeneraDEIB(4);
                        break;

                    case 10:
                    case 13:
                        ModeloNegocio.Transferencia.GeneraDEIB(3);
                        break;

                    case 11:
                    case 12:
                        ModeloNegocio.Transferencia.GeneraDEIB(2);
                        break;

                    case 14:
                        ModeloNegocio.Transferencia.GeneraTDIB(1, true, false);
                        break;

                    case 15:
                        ModeloNegocio.Transferencia.GeneraTDIB(3, true, false);
                        break;

                    case 16:
                        ModeloNegocio.Transferencia.GeneraHOLD(1);
                        break;

                    case 17:
                        ModeloNegocio.Transferencia.GeneraCTIB(false, true, 0);
                        break;

                    case 18:
                        ModeloNegocio.Transferencia.GeneraCTIB(false, true, 1);
                        break;

                    case 19:
                        ModeloNegocio.Transferencia.GeneraTDIB(1, false, true);
                        break;
                }

                PreparaGrid();
                IniciaBitacoraTransf();
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
            }         
        }

        private void IniciaBitacoraTransf()
        {
            //throw new NotImplementedException();
        }

        private void PreparaGrid()
        {
            dtMarco = new DataTable();

            dtMarco.Columns.Add("Descripcion", typeof(string));

            List<string> descripciones = new List<string>();
            descripciones.Add("Mensajes Swift MT103");
            descripciones.Add("Mensajes Swift MT198");
            descripciones.Add("Mensajes Swift MT202");
            descripciones.Add("Apertura de Cuentas");
            descripciones.Add("Mantenimiento de Cuentas");
            descripciones.Add("Depósitos por Sucursal");
            descripciones.Add("Deposito por TDD");
            descripciones.Add("Retiros por Sucursal");
            descripciones.Add("Retiro por TDD");
            descripciones.Add("Retiros por Orden de Pago en USD");
            descripciones.Add("Retiros por Orden de Pago en Otras Divisas");
            descripciones.Add("Traspasos (Misma Agencia)");
            descripciones.Add("Traspasos (Entre Agencias)");
            descripciones.Add("Operaciones Especiales");
            descripciones.Add("Time Deposit´s Houston");
            descripciones.Add("Time Deposit´s / TD´s Overnight G. Caimán");
            descripciones.Add("Holds");

            foreach (string desc in descripciones)
            {
                DataRow dr = dtMarco.NewRow();
                dr["Descripcion"] = desc;
                dtMarco.Rows.Add(dr);
            }

            dtgvMarco.DataSource = dtMarco;
            dtgvMarco.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgvMarco.Refresh();
            


            dtEnviarRecibir = new DataTable();


            dtEnviarRecibir.Columns.Add("Por Enviar 1", typeof(string));
            dtEnviarRecibir.Columns.Add("Por Recibir 1", typeof(string));
            dtEnviarRecibir.Columns.Add("Por Enviar 2", typeof(string));
            dtEnviarRecibir.Columns.Add("Por Recibir 2", typeof(string));
            dtEnviarRecibir.Columns.Add("Ultimo Envio", typeof(string));
            dtEnviarRecibir.Columns.Add("Proximo Envio", typeof(string));

            LlenaDtgvEnviarRecibir(dtEnviarRecibir.Columns.Count, dtMarco.Rows.Count);

            dtgvEnviarRecibir.DataSource = dtEnviarRecibir;
            dtgvEnviarRecibir.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dtgvEnviarRecibir.Refresh();


            

        }

        private void LlenaDtgvEnviarRecibir(int columnas, int tipos)
        {
            
            for (int i = 0; i < tipos; i++)
            {
                DataRow dr = dtEnviarRecibir.NewRow();
                for (int j = 0; j < columnas; j++)
                {
                    dr[j] = "--";
                }
                dtEnviarRecibir.Rows.Add(dr);
            }
        }
    }
}
