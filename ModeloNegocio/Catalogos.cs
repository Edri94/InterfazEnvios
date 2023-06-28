using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloNegocio
{
    public static class Catalogos
    {

        public static Datos.CLIENTE ClienteByCuenta(string cuenta)
        {
            using (Datos.CATALOGOSEntities contexto = new Datos.CATALOGOSEntities())
            {
                return contexto.CLIENTE.Where(w => w.cuenta_cliente == cuenta).FirstOrDefault();
            }
        }

        public static int ActualizaCliente(Datos.CLIENTE cliente)
        {
            using (Datos.CATALOGOSEntities contexto = new Datos.CATALOGOSEntities())
            {
                contexto.CLIENTE.Add(cliente);
                return contexto.SaveChanges();
            }
        }

        public static int BorrarClienteByCliente(Datos.CLIENTE cliente)
        {
            using (Datos.CATALOGOSEntities contexto = new Datos.CATALOGOSEntities())
            {
                contexto.CLIENTE.Remove(cliente);
                return contexto.SaveChanges();
            }
        }
    }
}
