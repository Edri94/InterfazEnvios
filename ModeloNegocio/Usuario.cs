using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloNegocio
{
    public class Usuario
    {
        public static List<Datos.USUARIO> Get()
        {
            try
            {
                using (Datos.CATALOGOSEntities context = new Datos.CATALOGOSEntities())
                {
                    return context.USUARIO.ToList();
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
