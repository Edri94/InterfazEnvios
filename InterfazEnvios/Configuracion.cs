using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfazEnvios
{
    public static class Configuracion
    {
        /// <summary>
        /// Obtiene parametro de configuracion
        /// </summary>
        /// <param name="key">Parametro a buscar dentro de un grupo</param>
        /// <param name="section">Grupo de parametros  de un parametro a buscar</param>
        /// <returns></returns>
        public static string getValueAppConfig(string key, string section = "")
        {
            if (section.Length >= 1)
            {
                return ConfigurationManager.AppSettings[$"{section}.{key}"];
            }
            else
            {
                return ConfigurationManager.AppSettings[$"{key}"];
            }

        }
    }
}
