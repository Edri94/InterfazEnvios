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
        /// Ejecuta SELECT a archivo en libreria en AS400 
        /// </summary>
        /// <param name="archivo">Tabla</param>
        /// <param name="libreria">Esquema</param>
        /// <returns></returns>
        public DataTable EjecutaSelect(string archivo, string libreria)
        {
            using (OdbcConnection cnn = new OdbcConnection(this.connection_str))
            {
                cnn.Open();

                OdbcCommand DbCommand = cnn.CreateCommand();
                DbCommand.CommandText = $"SELECT * FROM {libreria}.{archivo}";
                OdbcDataReader DbReader = DbCommand.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(DbReader);

                cnn.Close();
                return dt;
            }
           
        }


        /// <summary>
        /// Ejecuta un select con parametros
        /// </summary>
        /// <param name="archivo">Tabla</param>
        /// <param name="libreria">Esquema</param>
        /// <param name="where">string con el WHERE escribiendo el nombre de la columna y el parametro con un "?"</param>
        /// <param name="parametetros">array de OdbParameter pasando los parametros en el mismo orden en que se hizo en el where del parametro anterior de la funcion</param>
        /// <returns></returns>
        public DataTable EjecutaSelectConParametros(string archivo, string libreria, string where, OdbcParameter[] parametetros)
        {
            using (OdbcConnection cnn = new OdbcConnection(this.connection_str))
            {
                cnn.Open();

                OdbcCommand DbCommand = cnn.CreateCommand();
                DbCommand.CommandText = $"SELECT * FROM {libreria}.{archivo} WHERE {where}";
                DbCommand.Parameters.AddRange(parametetros);
                OdbcDataReader DbReader = DbCommand.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(DbReader);

                cnn.Close();
                return dt;
            }
        }

        /// <summary>
        /// Ejecuta una sentencia INSERT 
        /// </summary>
        /// <param name="archivo">Tabla</param>
        /// <param name="libreria">Esquema</param>
        /// <param name="into">array string con las columnas en el mismo orden que se pasara en el values y en el array de parametros</param>
        /// <param name="parametros">array de parametros pasados en el mismo orden que se paso en el string de las columnas</param>
        /// <returns></returns>
        public int EjecutaInsert(string archivo, string libreria, string[] into, object[] parametros)
        {
            if (into.Count() != parametros.Count())
            {
                throw new InvalidOperationException("No hay el mismo numero de parametros y de columnas descritas.");
            }

            using (OdbcConnection cnn = new OdbcConnection(this.connection_str))
            {
                cnn.Open();

                OdbcCommand DbCommand = cnn.CreateCommand();

                string values = "";
                string str_into = "";


                for(int i = 0; i < parametros.Count(); i++)
                {
                    str_into += "," + into[i];
                    values += ",?";
                }
                values = values.Remove(0, 1);
                str_into = str_into.Remove(0, 1);
              

                DbCommand.CommandText = $"INSERT INTO {libreria}.{archivo}({str_into}) VALUES({values})";

                OdbcParameter[] lista_parametros = new OdbcParameter[parametros.Count()];

                for(int i = 0; i < parametros.Count(); i++)
                {
                    lista_parametros[i] = new OdbcParameter("?", parametros[i]);
                }
                DbCommand.Parameters.AddRange(lista_parametros);

                int afectados = DbCommand.ExecuteNonQuery();

                cnn.Close();
                return afectados;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archivo">Tabla</param>
        /// <param name="libreria">Esquema</param>
        /// <param name="set"></param>
        /// <param name="where"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public int EjecutaUpdate(string archivo, string libreria, string[] set, string where, object[] parametros)
        {

            using (OdbcConnection cnn = new OdbcConnection(this.connection_str))
            {
                cnn.Open();

                OdbcCommand DbCommand = cnn.CreateCommand();

                string values = "";

                for (int i = 0; i < set.Count(); i++)
                {
                    values += $",{set[i]} = ?";
                }
                values = values.Remove(0, 1);

                DbCommand.CommandText = $"UPDATE {libreria}.{archivo} SET {values} WHERE {where}";

                OdbcParameter[] lista_parametros = new OdbcParameter[parametros.Count()];

                for (int i = 0; i < parametros.Count(); i++)
                {
                    lista_parametros[i] = new OdbcParameter("?", parametros[i]);
                }
                DbCommand.Parameters.AddRange(lista_parametros);

                int afectados = DbCommand.ExecuteNonQuery();

                cnn.Close();
                return afectados;
            }
        }

    }
}
