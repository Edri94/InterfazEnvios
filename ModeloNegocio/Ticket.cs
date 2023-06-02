using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ModeloNegocio.MqSeries;

namespace ModeloNegocio
{
    public static class Ticket
    {
        public static int BitacoraErrorMapeoSave(int lngErrorNum, string errorDesc, string archivoDest, TipoAccion eTipoAccion)
        {
            try
            {
                using (Datos.TICKETEntities context = new Datos.TICKETEntities())
                {

                    Datos.BITACORA_ERRORES_MAPEO error = new Datos.BITACORA_ERRORES_MAPEO
                    {
                        fecha_hora = DateTime.Now,
                        error_numero = lngErrorNum,
                        error_descripcion = errorDesc,
                        archivo_destino = archivoDest,
                        tipo_error = eTipoAccion.ToString()
                    };

                    context.BITACORA_ERRORES_MAPEO.Add(error);
                    int resultado = context.SaveChanges();
                    return resultado;
                }
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
                return -1;
            }
        }

        public static List<Datos.REPORTE_SWIFT_MT202> GetReporteSwiftMT202(DateTime fecha, int agencia1, int agencia3)
        {
            using (TICKETEntities contexto = new TICKETEntities())
            {
                
                List<REPORTE_SWIFT_MT202> rstMT202 = (List<REPORTE_SWIFT_MT202>)(
                    from R in contexto.REPORTE_SWIFT_MT202
                    join B in contexto.BITACORA_ENVIO_SWIFT_MT202 on R.num_rep equals B.num_rep
                    where R.fecha_reporte >= fecha
                    && R.fecha_reporte <= fecha
                    && R.agencia == agencia1 || R.agencia == agencia3
                    && B.status_envio == 0
                    orderby R.fecha_reporte
                    select new List<REPORTE_SWIFT_MT202>()
                );
                return rstMT202;
            }
        }
    }
}
