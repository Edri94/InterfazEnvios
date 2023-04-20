using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloNegocio
{
    /// <summary>
    /// Devuelve la fecha origen con la diferencia de tiempo indicada en el parametro
    /// </summary>
    public static class Funciones
    {
        public static DateTime Fecha(string origen, DateTime fechacierre, int dias = 0)
        {
            DateTime fecha;

            if(origen.Trim() == "")
            {
                fecha = Parametro.FechaServidor();
            }
            else if(origen.Contains("FIN"))
            {
                fecha = fechacierre;
            }
            else if(!DateTime.TryParse(origen, out fecha))
            {
                fecha = Parametro.FechaServidor();
            }

            if(dias > 0)
            {
                fecha = fecha.AddDays(dias);
            }


            return fecha;
        }
    }
}
