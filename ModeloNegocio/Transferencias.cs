using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloNegocio
{
    public class Transferencias
    {
        private const string TABLE = "TICKET.Transferencia.FIBIPC";
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

        public static List<Datos.FIBIPC> FibipcFindAll( )
        {
            try
            {
                using (Datos.TICKETEntities context = new Datos.TICKETEntities())
                {
                    List<Datos.FIBIPC> resultados = context.Database.SqlQuery<Datos.FIBIPC>($@"
                        SELECT 
                            archivo,
                            fecha
                        FROM
                            {TABLE}").ToList();

                    return resultados;
                }
            }
            catch (Exception ex)
            {
                Log.Escribe(ex);
                return null;
            }
        }

        public static int FibipcSave(string archivo, DateTime fecha)
        {
            try
            {
                using (Datos.TICKETEntities context = new Datos.TICKETEntities())
                {
                    int resultado = context.Database.ExecuteSqlCommand($@"INSERT INTO {TABLE}(archivo, fecha) VALUES(@archivo, @fecha)", new SqlParameter("@archivo", archivo), new SqlParameter("@fecha", fecha));

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
