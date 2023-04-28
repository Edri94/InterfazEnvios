using Datos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloNegocio
{
    public static class Parametro
    {
        public static Configuracion confg;

        public static bool mbTransStatus;

        public static DateTime gsFecOp1;
        public static DateTime gsFecOp2;
        public static DateTime gsFecOp3;
        public static DateTime gsFecOp4;

        public static DateTime gsFecResp;
        public static DateTime gsFecSV1;
        public static DateTime gsFecSV2;


        public static Datos.Parametros gsTotMT103;
        public static Datos.Parametros gsTotMT202;
        public static Datos.Parametros gsTotMT198;
        public static Datos.Parametros gsTotClientes;

        public static Datos.Parametros gsTotMantto;
                     
        public static Datos.Parametros gsTotDepRet;
        public static Datos.Parametros gsTotHOLDRet;
        public static Datos.Parametros gsTotROPUSD;
        public static Datos.Parametros gsTotTraspasos;
        public static Datos.Parametros gsTotTDs;

        public static Int16 gsPaso;
        public static Int16 gsBackup;


        public static Datos.Parametros GetParametrizacion(string codigo)
        {
            try
            {
                using (Datos.TICKETEntities context = new Datos.TICKETEntities())
                {
                    Datos.Parametros resultado = context.Database.SqlQuery<Datos.Parametros>($@"
                        SELECT 
                            A.valor as [valor]
                            ,B.error_saldos as [error_saldos]
                            ,B.fecha_sistema as [fecha_sistema]
                        FROM

                            TICKET.dbo.PARAMETRIZACION as A
                            , TICKET.dbo.PARAMETROS AS B
                        WHERE A.codigo = @codigo", new SqlParameter("codigo", codigo)).FirstOrDefault();

                    return resultado;
                }
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
                return null;
            }
        }


        public static Int16 GetParametro(string parametro)
        {
            try
            {
                using (Datos.TICKETEntities context = new Datos.TICKETEntities())
                {
                    Int16 resultado = context.Database.SqlQuery<Int16>($@"
                        SELECT 
                            {parametro}
                        FROM
                            TICKET.dbo.PARAMETROS").FirstOrDefault();

                    return resultado;
                }
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
                return -1;
            }
        }

        /// <summary>
        /// Obtiene la fecha del SQL Server
        /// </summary>
        /// <returns></returns>
        public static DateTime FechaServidor()
        {
            using (Datos.TICKETEntities context = new Datos.TICKETEntities())
            {
                DateTime resultado = context.Database.SqlQuery<DateTime>($@"
                   SELECT GETDATE()").FirstOrDefault();

                return resultado;
       
            }

        }

        public static bool DespliegaDatos()
        {
            using (Datos.TICKETEntities context = new Datos.TICKETEntities())
            {
                DateTime fecha_Servidor = FechaServidor();

                Datos.ParametrosEjecucion resultado = context.Database.SqlQuery<Datos.ParametrosEjecucion>($@"
                    SELECT 
	                    getdate() AS [Fecha_Servidor],     
	                    fecha_sistema AS [Fecha_Sistema], 
	                    ISNULL(no_tran,1) AS [MT103],                             
	                    ISNULL(no_ctez,1) AS [CTIB],                             
	                    ISNULL(no_derz,1) AS [DRT],                             
	                    ISNULL(no_cdsz,1) AS [TDs],                             
	                    ISNULL(no_hosz,1) AS [HOLD],                             
	                    ISNULL(no_swag,0) AS [Swift],                             
	                    ISNULL(no_mt202,1) AS [MT202]                             
                    FROM TICKET.dbo.PARAMETROS").FirstOrDefault();

                if(resultado != null)
                {
                    if (fecha_Servidor.ToString("dd-MM-yyyy") != resultado.Fecha_Sistema.ToString("dd-MM-yyyy"))
                    {
                        Log.Escribe("La fecha en parámetros no es la misma que la fecha del servidor!");
                    }
                    if (fecha_Servidor.ToString("dd-MM-yyyy") != DateTime.Now.ToString("dd-MM-yyyy"))
                    {
                        Log.Escribe("La fecha de la PC no es la misma que la fecha del servidor!");
                    }
                }
                else
                {
                    Log.Escribe("No existen datos en la tabla de PARAMETROS", "Error");
                    return false;
                }

                Datos.Parametros to_tregs_fecha = GetParametrizacion("TOTREGSFECHA");
                Datos.Parametros trans_status = GetParametrizacion("TRANSSTATUS");

                //Si la fecha de transferencia es hoy

                DateTime tregs_fecha = Convert.ToDateTime(DateTime.ParseExact(to_tregs_fecha.valor.Trim(), "MM-dd-yyyy", CultureInfo.InvariantCulture));
                if ((tregs_fecha - resultado.Fecha_Servidor).Days == 0)
                {
                    
                    if (trans_status.valor.Trim() == "CLOSED")
                    {
                        mbTransStatus = false;
                        Log.Escribe("Transferencia Cerrada!");                 
                    }
                    else
                    {
                        mbTransStatus = true;
                    }
                }
                else
                {
                    if(trans_status.valor.Trim() == "CLOSED")
                    {
                        //Inicializa los parámetros de ejecucion
                        ActualizaParametro("TOTREGSFECHA", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                        ActualizaParametro("TOTREGSFECHA", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                        ActualizaParametro("TRANSSTATUS", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                        ActualizaParametro("TOTREGSMT103", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                        ActualizaParametro("TOTREGSMT202", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                        ActualizaParametro("TOTREGSSWIFT", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                        ActualizaParametro("TOTREGSCTES", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));

                        //DGI (EDS) MANTENIMIENTO
                        ActualizaParametro("TOTREGSMANT", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));

                        ActualizaParametro("TOTREGSDEPRET", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                        ActualizaParametro("TOTREGSHOLDS", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                        ActualizaParametro("TOTREGSROPD", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                        ActualizaParametro("TOTREGSTRASP", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));
                        ActualizaParametro("TOTREGSTDS", resultado.Fecha_Servidor.ToString("yyyy-MM-dd hh:mm:ss"));

                        //EPH020305                      
                        gsFecOp1 = fecha_Servidor.AddMinutes(10);
                        gsFecOp2 = gsFecOp1.AddMinutes(5);
                        gsFecOp3 = gsFecOp2.AddMinutes(5);
                        gsFecOp4 = gsFecOp3.AddMinutes(5);

                        gsFecResp = Funciones.Fecha("FIN", confg.fechaCierre);
                        gsFecSV1 = Funciones.Fecha("FIN", confg.fechaCierre);
                        gsFecSV2 = Funciones.Fecha("FIN", confg.fechaCierre);
                        confg.fechaCierre = new DateTime(fecha_Servidor.Year, fecha_Servidor.Month, fecha_Servidor.Day) + confg.horaCierre; 
                    }
                }

                //Lee el numero de operaciones procesadas
                gsTotMT103 = GetParametrizacion("TOTREGSMT103");
                gsTotMT202 = GetParametrizacion("TOTREGSMT202");
                gsTotMT198 = GetParametrizacion("TOTREGSSWIFT");
                gsTotClientes = GetParametrizacion("TOTREGSCTES");
                //DGI (EDS) Mantenimiento
                gsTotMantto = GetParametrizacion("TOTREGSMANT");

                gsTotDepRet = GetParametrizacion("TOTREGSDEPRET");
                gsTotHOLDRet = GetParametrizacion("TOTREGSHOLDS");
                gsTotROPUSD = GetParametrizacion("TOTREGSROPD");
                gsTotTraspasos = GetParametrizacion("TOTREGSTRASP");
                gsTotTDs = GetParametrizacion("TOTREGSTDS");

                CargaSalVen.CalcDiaSig();
                
                VerificaRespaldo();

                return true;

            }
        }

        private static bool VerificaRespaldo()
        {
            gsPaso = GetParametro("error_saldos_diarios");

            if(gsPaso != -1)
            {
                gsBackup = GetParametro("backup_saldos_diarios");

                if(gsBackup == -1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private static void ActualizaParametro(string parametro, string valor)
        {
            using (Datos.TICKETEntities context = new Datos.TICKETEntities())
            {
                Datos.PARAMETRIZACION param = context.PARAMETRIZACION.Where(w => w.codigo == parametro).FirstOrDefault();

                if(param != null)
                {
                    param.valor = valor;
       
                    context.Entry(param).State = System.Data.Entity.EntityState.Modified;
                    int actualizados = context.SaveChanges();

                    if(actualizados < 1)
                    {
                        Log.Escribe($"Ocurrio un error al actualziar el parametro {parametro}");
                    }
                }
            }
        }
    }

}
