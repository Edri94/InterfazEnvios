using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ConexionAS400
    {
        private string user;
        private string password;
        private string dsn;
        private string connection_str; 


        public ConexionAS400(string user, string password, string dsn)
        {
            this.user = user;
            this.password = password;
            this.dsn = dsn;

            this.connection_str = $"DSN={dsn}; UID={user}; PWD={password};";

        }

        public bool PruebaConexion()
        {
            try
            {
                using (OdbcConnection cnn = new OdbcConnection(this.connection_str))
                {
                    cnn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        /// <summary>
        /// Ejecuta consulta a ODBC
        /// </summary>
        /// <param name="query">Query a ejecutar</param>
        /// <returns>Datatable resultado del query</returns>
        public DataTable EjecutaSelect(string query)
        {
            using (OdbcConnection cnn = new OdbcConnection(this.connection_str))
            {
                cnn.Open();

                OdbcCommand DbCommand = cnn.CreateCommand();
                DbCommand.CommandText = query;
                OdbcDataReader DbReader = DbCommand.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(DbReader);

                cnn.Close();
                return dt;
            }
           
        }

        /// <summary>
        /// Ejecuta consulta com parametros a ODBC
        /// </summary>
        /// <param name="query">Query a ejecutar</param>
        /// <param name="parametetros">Parametros del query</param>
        /// <returns>Datatable resultado del query</returns>
        public DataTable EjecutaSelectConParametros(string query, OdbcParameter[] parametetros)
        {
            using (OdbcConnection cnn = new OdbcConnection(this.connection_str))
            {
                cnn.Open();

                OdbcCommand DbCommand = cnn.CreateCommand();
                DbCommand.CommandText = query;
                DbCommand.Parameters.Add(parametetros);
                OdbcDataReader DbReader = DbCommand.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(DbReader);

                cnn.Close();
                return dt;
            }
        }


        public int EjecutaActualizacion(string query)
        {
            using (OdbcConnection cnn = new OdbcConnection(this.connection_str))
            {
                cnn.Open();

                OdbcCommand DbCommand = cnn.CreateCommand();
                DbCommand.CommandText = query;
                int afectados = DbCommand.ExecuteNonQuery();

                cnn.Close();
                return afectados;
            }
        }

        public int EjecutaActualizacion(string query, OdbcParameter[] parametros)
        {

            using (OdbcConnection cnn = new OdbcConnection(this.connection_str))
            {
                cnn.Open();

                OdbcCommand DbCommand = cnn.CreateCommand();
                DbCommand.CommandText = query;

                foreach (OdbcParameter parametro in parametros)
                {
                    DbCommand.Parameters.Add(parametro);
                }
                int afectados = DbCommand.ExecuteNonQuery();

                cnn.Close();
                return afectados;
            }
        }

    }
}
