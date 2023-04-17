using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloNegocio
{
    public static class Parametro
    {

        public static Datos.Parametros GetParamnetro(string codigo)
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
    }

}
