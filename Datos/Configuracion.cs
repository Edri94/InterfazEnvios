using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class Configuracion
    {
        public Configuracion()
        {

        }


        public string gsAppPassword { get; set; }
        public string ambiente { get; set; }
        public string pathFtpApp { get; set; }
        public string sna620 { get; set; }
        public string libSwift { get; set; }
        public string libSaldos { get; set; }
        public string libDefault { get; set; }
        public string sendLaops { get; set; }
        public string sendGcOps { get; set; }
        public string sendAS400 { get; set; }
        public string slashMT103 { get; set; }
        public bool chkEq { get; set; }
        public bool chkSw { get; set; }
        public bool chkEnvEqt { get; set; }
        public bool chkEnvSwf { get; set; }
        public bool chkClose { get; set; }
        public bool chkSaldosHc { get; set; }
        public bool chkSaldosGc { get; set; }
        public bool chkTdsHo { get; set; }
        public bool chkTdsGc { get; set; }
        public bool chkOverGc { get; set; }
        public int periodo0 { get; set; }
        public int periodo1 { get; set; }
        public int periodo2 { get; set; }
        public int periodo3 { get; set; }
        public int periodoResp { get; set; }
        public int periodoSaldo0 { get; set; }
        public int periodoSaldo1 { get; set; }
        public DateTime fechaCierre { get; set; }
        public DateTime fechaRespaldo { get; set; }
        public DateTime fechaSV1 { get; set; }
        public DateTime fechaSV2 { get; set; }
        public TimeSpan horaCierre { get; set; }
        public DateTime horaCierreAuto { get; set; }
        public DateTime fechaOp1 { get; set; }
        public DateTime fechaOp2 { get; set; }
        public DateTime fechaOp3 { get; set; }
        public DateTime fechaOp4 { get; set; }
        public string pathEquation { get; set; }
        public string pathSwift { get; set; }
        public string pathModels { get; set; }
        public string pathApp { get; set; }
        public string pathSaldos { get; set; }
        public bool envioBl { get; set; }
        public int periodoBl { get; set; }
        public DateTime fechaBl { get; set; }
        public bool execStoredSaldos { get; set; }
        public bool[] envio { get; set; }
        public int noIntentos { get; set; }
        public string AppName { get; set; }
        public string mqManager { get; set; }
        public string mqEscribir { get; set; }
        public string mqLeer { get; set; }
        public string mqReporte { get; set; }

        public string usr400 { get; set; }
        public string pswd400 { get; set; }
        public string dsn400 { get; set; }
        public string usrSql { get; set; }
        public string pswdSql { get; set; }
        public string dataSource { get; set; }



        /// <summary>
        /// Obtiene parametro de configuracion
        /// </summary>
        /// <param name="key">Parametro a buscar dentro de un grupo</param>
        /// <param name="section">Grupo de parametros  de un parametro a buscar</param>
        /// <returns></returns>
        public string getValueAppConfig(string key, string section = "")
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

        public bool SetParameterAppSettings(string key, string value, string section = "")
        {

            string nombre_appconfig = $"{AppName}.exe.config";

            try
            {
                string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string[] appPath_arr = appPath.Split('\\');


                if (File.Exists(System.IO.Path.Combine(appPath, nombre_appconfig)))
                {
                    string configFile = System.IO.Path.Combine(appPath, nombre_appconfig);
                    ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
                    configFileMap.ExeConfigFilename = configFile;
                    System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
                    if (section.Length > 0)
                    {
                        config.AppSettings.Settings[$"{section}.{key}"].Value = value;
                    }
                    else
                    {
                        config.AppSettings.Settings[key].Value = value;
                    }
                    config.Save();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// Encriptacion de la seccion de connectionStrings en el app.config
        /// </summary>
        public void EncryptConnectionString()
        {
            // Abre el archivo de configuración de la aplicación
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // Obtiene la sección de conexión
            ConnectionStringsSection connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");

            //foreach (ConnectionStringSettings cnn in connectionStringsSection.ConnectionStrings)
            //{
            //    string cnn_get = cnn.ToString();
            //}

            // Encripta la sección de conexión si no está encriptada
            if (!connectionStringsSection.SectionInformation.IsProtected)
            {
                connectionStringsSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                config.Save();
            }
        }

        /// <summary>
        /// Desencriptacion de la seccion de connectionStrings en el app.config
        /// </summary>
        public void DecryptConnectionString()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConnectionStringsSection connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");

            if (connectionStringsSection.SectionInformation.IsProtected)
            {
                connectionStringsSection.SectionInformation.UnprotectSection();
                config.Save();
            }
        }
    }
}
