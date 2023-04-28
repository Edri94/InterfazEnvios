using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloNegocio
{
    public class Transferencias
    {
        public static List<Datos.TIPO_MANTENIMIENTO_CUENTA> cllMantenimiento;

        public static bool LlenarColeccion()
        {
            using(Datos.TICKETEntities context = new Datos.TICKETEntities())
            {
                cllMantenimiento = context.TIPO_MANTENIMIENTO_CUENTA.ToList();

                if(cllMantenimiento != null)
                {
                    return true;
                }
                else
                {
                    Log.Escribe("No es posible cargar la informacion del catalogo de Mantenimeinto");
                    return false;
                }
            }
        }
    }
}
