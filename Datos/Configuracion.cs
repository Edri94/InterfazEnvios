using System;
using System.Collections.Generic;
using System.Configuration;
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
