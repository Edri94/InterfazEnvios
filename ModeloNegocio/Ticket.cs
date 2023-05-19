using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Datos.MqSeries;

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
    }
}
