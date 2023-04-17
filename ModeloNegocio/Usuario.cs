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
