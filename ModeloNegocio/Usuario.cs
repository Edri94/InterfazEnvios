using MNICript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloNegocio
{
    public class Usuario
    {
        static clsEncripta crpt;

        public static List<Datos.USUARIO> Get()
        {
            crpt = new clsEncripta();

            try
            {
                //using (Datos.TICKETEntities context = new Datos.TICKETEntities())
                //{
                //    DateTime fecha1 = DateTime.Now.AddDays(-1);
                //    DateTime fecha2 = DateTime.Now.AddDays(1);
                //    int agencia1 = 1;
                //    int agencia3 = 3;

                //    List<Datos.REPORTE_SWIFT_MT202> i_InfoMT202 = (
                //        from a in context.REPORTE_SWIFT_MT202
                //        join b in context.BITACORA_ENVIO_SWIFT_MT202 on a.num_rep equals b.num_rep
                //        where 
                //            a.fecha_reporte  >= fecha1
                //            && 
                //            a.fecha_reporte <= fecha2
                //            &&
                //            b.status_envio == 0
                //            && 
                //            (a.agencia == agencia1 || a.agencia == agencia3)
                //        select a).ToList();
                //}


                using (Datos.CATALOGOSEntities context = new Datos.CATALOGOSEntities())
                {
                    List<Datos.USUARIO> usuarios = context.USUARIO.ToList();
                    return usuarios;
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return null;
            }
            
        }
    }
}
