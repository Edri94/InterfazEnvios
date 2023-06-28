using Datos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloNegocio
{
    public class Pendientes
    {

        public static void CalculaInfo(int rowIndex, int columnIndex)
        {
            string aviso = "";
            int seccion;

            switch (rowIndex)
            {
                case 1:
                    aviso = "Buscando Mensajes Swift MT103 Pendientes...";
                    break;
                case 2:
                    aviso = "Buscando Mensajes Swift MT198 Pendientes...";
                    break;
                case 3:
                    aviso = "Buscando Mensajes Swift MT202 Pendientes...";
                    break;
                case 4:
                    aviso = "Buscando Aperturas Pendientes...";
                    break;
                case 5:
                    aviso = "Buscando Mantenimiento Pendientes...";
                    break;
                case 6:
                    aviso = "Buscando Depositos Por Sucursal Pendientes...";
                    break;
                case 7:
                    aviso = "Buscando Depositos por TDD Pendientes...";
                    break;
                case 8:
                    aviso = "Buscando Retiros Por Sucursal Pendientes...";
                    break;
                case 9:
                    aviso = "Buscando Retiros por TDD Pendientes...";
                    break;
                case 10:
                    aviso = "Buscando Órdenes de Pago en USD Pendientes...";
                    break;
                case 11:
                    aviso = "Buscando Órdenes de Pago en Otras Divisas Pendientes...";
                    break;
                case 12:
                    aviso = "Buscando Traspasos Misma Agencia Pendientes...";
                    break;
                case 13:
                    aviso = "Buscando Traspasos Entre Agencias Pendientes...";
                    break;
                case 14:
                    aviso = "Buscando Operaciones Especiales Pendientes...";
                    break;
                case 15:
                    aviso = "Buscando TDs de HO. Pendientes...";
                    break;
                case 16:
                    aviso = "Buscando TDs de G.C. Pendientes...";
                    break;
                case 17:
                    aviso = "Buscando Holds Pendientes...";
                    break;
            }

            seccion = CalculaSeccion(rowIndex, columnIndex);

            switch (columnIndex)
            {
                case 0:
                case 2:
                    ActualizaPendientes(2, seccion);

                    break;
                case 1:
                case 3:
                   ActualizaPendientes(3, seccion);

                    break;
            }

            Log.Escribe(aviso);
        }

        public static void ActualizaPendientes(int sendReceive, int seccion)
        {
            using (TICKETEntities contexto = new TICKETEntities())
            {
                PonGuion(sendReceive, seccion);

                ObjectResult<Datos.sp_i_Ops_Enviar_Recibir_Result> resultado = contexto.sp_i_Ops_Enviar_Recibir(sendReceive, seccion, DateTime.Now.ToString("yyyy-MM-dd"));

                if (resultado != null)
                {
                    if (seccion == 20)
                    {
                        if (sendReceive == 2)
                        {
                            for (int i = 0; i <= 19; i++)
                            {

                            }
                        }
                        else if (sendReceive == 3)
                        {
                            for (int i = 0; i <= 19; i++)
                            {

                            }
                        }
                    }
                    else if (seccion < 20)
                    {
                        if (sendReceive == 2)
                        {

                        }
                        else if (sendReceive == 3)
                        {

                        }
                    }
                }
                else
                {
                    PonGuionCero(sendReceive, seccion);
                }
            }
        }

        public static void PonGuionCero(int sendReceive, int seccion)
        {
            throw new NotImplementedException();
        }

        public static void PonGuion(int sendReceive, int seccion)
        {
            throw new NotImplementedException();
        }

        public static int CalculaSeccion(int rowIndex, int columnIndex)
        {
            throw new NotImplementedException();
        }
    }
}
