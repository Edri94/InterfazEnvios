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

        public static Datos.ParametrosEjecucion GetParametrosEjecucion()
        {
            using (Datos.TICKETEntities context = new Datos.TICKETEntities())
            {
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

                return resultado;
            }
        }

        public static void ActualizaParametro(string parametro, string valor)
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
