using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModeloNegocio;
using Datos;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace DLLTRAN
{
    public class Tran
    {

        Configuracion config = new Configuracion();

        string cnn;
        string gstrSQL;
        int agencia1;
        int agencia3;
        string archivo;
        string rutaTotal;
        string fechaTrabajo;
        string[] arrFormato;
        string tipoAmbiente;
        byte slashVerifica;
        string[] arrOperacion;
        string msSendROPD;
        string strRetiros;      //Complemento del formato de swift retiros Hou
        string strDepositos;    //Complemento del formato de swift depositos Hou
        string strRetirosC;     //Complemento del formato de swift retiros Cayman
        string strDepositosC;   //Complemento del formato de swift depositos Cayman
        string[] arrUETR;       //BPSA, descripcion: Arreglo para guardar UETR en base de datos. Estandares 2018
        bool banderaGPIMT103; //BPSA y LEPR, descripcion: Bandera para mostrar el campo 111 cuando su valor es True y cuando es False no se muestra. Estandares 2018
        bool banderaPruebasGPI33B;  //BPSA, descripcion: Bandera para mostrar el campo 33B cuando su valor es True y cuando es False no se muestra. SWIFT GPI
        string rutaUETR_TXT;     //LEPR ruta donde leera el UETR generado
        string rutaUETR_BAT;     //LEPR ruta donde ejecutara el Bat
        string logUETR;          //LEPR Muestra los generado por el UETR
        string rutaLog;          //LEPR Muestra la ruta del log
        string rutaUETR;         //LEPR Muestra la ruta donde se genera el UETR
                                 // lib             As New Libreria

        //OLIVIA FARIAS 07/SEP/21
        //SE REALIZA CAMBIO DE PARA EL INDICADOR DE LOS MENSAJES SWIFT DE LOGICAL TERMINAL DE A POR X
        string srtLTSWIFT;        //CODIGO PARA SWIFT LOGICAL TERMINAL

        //Funcion para utilizar sleep para ejecutar el bat. Estandares 2018
        private void Sleep(int dwMilliseconds)
        {
            Thread.Sleep(dwMilliseconds);
        }


        //Función para ejecutar cualquier tipo de aplicacion. Estandares 2018
        private long ShellExecute(long hwnd, string lpOperation,
                            string lpFile, string lpParameters,
                            string lpDirectory, long nShowCmd)
        {
            long result = 0;
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "shell32.dll",
                Arguments = $"\"{hwnd}\" \"{lpOperation}\" \"{lpFile}\" \"{lpParameters}\" {lpDirectory} {nShowCmd}",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            Process process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
            result = long.Parse(process.StandardOutput.ReadToEnd());
            process.WaitForExit();
            return result;
        }


        public string TRANGeneration(string fecha,
                                            int ag1,
                                            int ag3,
                                    string cnnConexion,
                                    string nombreArchivo,
                                    string rutaCompleta,
                                    string ambiente,
                                    byte slash,
                                    string SendROP)
        {
            try
            {
                string result = "";
                string infoGenerada;
                string[] verificacion;

                //Inicia variables
                verificacion = new string[0];
                agencia1 = ag1;
                agencia3 = ag3;
                archivo = nombreArchivo;
                rutaTotal = rutaCompleta;
                fechaTrabajo = fecha;
                tipoAmbiente = ambiente;
                slashVerifica = slash;
                msSendROPD = SendROP;


                //Inicializa el arreglo para el armado del mensaje
                if (iniciaArreglos(cnnConexion))
                {
                    //Obtenemos la informacion y armamos el archivo plano
                    infoGenerada = obtieneInformacionTRAN(cnnConexion);
                    //Verificamos el correcto funcionamiento de la generacion del archivo
                    //Para actualizar el status y modificar las bitacoras.
                }




                return result;
            }
            catch (Exception ex)
            {
                string message = "Error en el modulo BorrarArchivo libreria dll MT202 -> ";
                Log.Escribe(ex, message);
                return "";
            }
        }

        public string obtieneInformacionTRAN(string cnnConexion) 
        {
            string result = "";
            return result;
        }

        private string UETR(string soperacion)
        {
            /*---------------------------------------------------------------------------------------------------- -
             BAPS y LEPR 28 - 09 - 2018 Se genera UETR por medio del archivo bach y lo deposita en un txt de paso, para el campo 121 del mensaje de acuerdo al Estandar 2018
            --------------------------------------------------------------------------------------------------- -

            'Instruccion que ejecuta el bat. Estadares 2018
            'Nuestro bat recibe 2 parametros (soperacion & " 1"):
            'soperacion = numero de operacion o TIcket obtenida del recordset
            '1 = MT103 y 2 = MT202*/
            string result = "";

            long res;
            string StrRuta;
            string strLinea;
            int contador = 0;

            rutaUETR_TXT = config.getValueAppConfig("rutaUETR_TXT", "PARAMETROS"); // LEPR, descripcion: Obtiene la ruta donde se va a escribir el UETR generado
            rutaUETR_BAT = config.getValueAppConfig("rutaUETR_BAT", "PARAMETROS"); // LEPR, descripcion: Obtiene la ruta donde se encuentra el archivo Bat
            logUETR = config.getValueAppConfig("logUETR", "PARAMETROS"); // LEPR, descripcion: Obtiene el valor para mostrar o no el log generado por el UETR
            rutaLog = config.getValueAppConfig("rutaLog", "PARAMETROS");  //LEPR, descripcion: Obtiene ruta del log UETR
            rutaUETR = config.getValueAppConfig("rutaUETR", "PARAMETROS");  //LEPR, descripcion: Obtiene ruta del UETR

            //Lee variable UETR del UETR103.txt (txt de paso) que genero el .bat. Estadares 2018
            StrRuta = rutaUETR_TXT + @"\UETR103.txt"; //"C:\UETR\UETR_TXT\UETR103.txt"

            // Crear un objeto de FileSystemInfo
            FileSystemInfo o_Fso = new FileInfo(StrRuta);

            //Borra el contenido del archivo
            StreamWriter f1 = new StreamWriter(StrRuta);

            //"y finalmente con esto cierras el archivo"
            f1.Close();

            //res = ShellExecute(0, "", "C:\UETR\UETRV4_BAT.bat ", soperacion & " 2", "", 0)
            res = ShellExecute(0, "", rutaUETR_BAT + "\\UETRV4_BAT.bat", soperacion + " 1 " + logUETR + " " + rutaLog + " " + rutaUETR, "", 0);

            Sleep(1500);

            // Abrir el archivo en modo de escritura
            StreamReader archivo = new StreamReader(StrRuta);

            // Leer el archivo en un bucle hasta que se cumpla la condición
            do
            {
                contador++;

                if (contador == 1000)
                {
                    // Salir del bucle si se cumple la condición
                    result = "Error al generar UETR, revise direcciones y realice prueba local de BAT -> ";
                    Log.Escribe(result);
                    return result;
                }

                // Leer una línea del archivo
                strLinea = archivo.ReadLine();

            } while (archivo.BaseStream.Length < 20);

            // Cerrar el archivo
            archivo.Close();

            // Abrir el archivo en modo de lectura
            StreamReader archivoLectura = new StreamReader(StrRuta);
            // Leer el archivo en un bucle hasta que se llegue al final
            while (!archivoLectura.EndOfStream)
            {
                result = archivoLectura.ReadLine();
            }

            // Cerrar el archivo
            archivoLectura.Close();

            // Abrir el archivo en modo de escritura
            StreamWriter archivo1 = new StreamWriter(StrRuta);

            // Cerrar el archivo
            archivo1.Close();
            return result;
        }


        private bool generaArchivoPlano(string rstDatos, string numeroRegistros, string cnnConexion) 
        {
            bool result = false;

            return result;
        }

        private bool actualizaStatus(string cnnConexion) 
        {
            bool result = false;

            return result;
        }

        private bool insertaUETR(string cnnConexion = "")
        {
            bool result = false;
            try
            {
                using (TICKETEntities contexto = new TICKETEntities())
                {
                    using (var trans = contexto.Database.BeginTransaction())
                    {
                        int cont = 0;
                        foreach (var item in arrOperacion)
                        {
                            contexto.TICKET_UETR.Add(
                                new TICKET_UETR
                                {
                                    operacion = int.Parse(arrOperacion[cont]),
                                    UETR = arrUETR[cont],
                                    fecha_actual = DateTime.Now,
                                    usuario = "InterfazEnvios"
                                });
                            cont++;
                        }

                        contexto.SaveChanges();
                        trans.Commit();

                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                string message = "Error en el modulo insertaUETR libreria dll MT202 -> ";
                Log.Escribe(ex, message);
                return result;
            }
            return result;
        }

        private bool iniciaArreglos(string cnnConexion) 
        {
            bool result = false;

            return result;
        }

        private string ValidaCadena(string sTexto)
        {
            string lsCaracter = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ,.-+/1234567890=()?:{}";
            string lsTexto = sTexto;

            for (int lnIndice = 0; lnIndice < sTexto.Trim().Length; lnIndice++)
            {
                if (sTexto[lnIndice] == 'Ñ')
                {
                    lsTexto = lsTexto.Remove(lnIndice, 1).Insert(lnIndice, "N");
                }
                else if (sTexto[lnIndice] == 'ñ')
                {
                    lsTexto = lsTexto.Remove(lnIndice, 1).Insert(lnIndice, "n");
                }
                else
                {
                    int lnPosicion = lsCaracter.IndexOf(sTexto[lnIndice]);
                    if (lnPosicion == -1)
                    {
                        lsTexto = lsTexto.Remove(lnIndice, 1).Insert(lnIndice, " ");
                    }
                }
            }

            return lsTexto;
        }
    }
}
