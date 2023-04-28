using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ModeloNegocio
{
    public static class Log
    {
        public static bool EscribeLog;
        public static string RutaLog;
        public static Exception error;
        public static string Archivo_Actual;
        public static string Nombre_App;

        public static void Escribe(string vData, string tipo = "Mensaje")
        {
            try
            {
                if (!Directory.Exists(Log.RutaLog))
                {
                    Directory.CreateDirectory(Log.RutaLog);
                }

                StackTrace trace = new StackTrace(StackTrace.METHODS_TO_SKIP + 2);
                StackFrame frame = trace.GetFrame(0);
                MethodBase caller = frame.GetMethod();


                Archivo_Actual = DateTime.Now.ToString("ddMMyyyy") + "_" + Nombre_App + ".log";


                if (EscribeLog)
                {
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(RutaLog, Archivo_Actual), append: true))
                    {
                        vData = $"[{DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss")}]  {tipo}:  {vData}";
                        outputFile.WriteLine(vData);
                    }

                }
            }
            catch (IOException ex)
            {
                error = ex;
            }

        }

        public static void Escribe(Exception ex, string tipo = "Error")
        {
            try
            {
                if (!Directory.Exists(Log.RutaLog))
                {
                    Directory.CreateDirectory(Log.RutaLog);
                }

                StackTrace trace = new StackTrace(StackTrace.METHODS_TO_SKIP + 2);
                StackFrame frame = trace.GetFrame(0);
                MethodBase caller = frame.GetMethod();

                string clase = caller.ReflectedType.Name;
                string funcion = caller.Name;
                string vData = "";
                //clase = "";

                Archivo_Actual = DateTime.Now.ToString("ddMMyyyy") + "_" + Nombre_App + ".log";


                if (EscribeLog)
                {
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine(RutaLog, Archivo_Actual), append: true))
                    {
                        vData = $"[{DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss")}] {(char)13}" +
                            $"*{tipo} desde {funcion}:  {ex.Message} {(char)13}" +
                            $"*InnerException: {ex.InnerException} {(char)13}" +
                            $"*Source: {ex.Source}  {(char)13}" +
                            $"*Data: {ex.Data}  {(char)13}" +
                            $"*HelpLink: {ex.HelpLink}  {(char)13}" +
                            $"*StackTrace: {ex.StackTrace}  {(char)13}" +
                            $"*HResult: {ex.HResult}  {(char)13}" +
                            $"*TargetSite: {ex.TargetSite}  {(char)13}";
                        outputFile.WriteLine(vData);
                    }

                }
            }
            catch (IOException e)
            {
                error = e;
            }

        }


    }
}
