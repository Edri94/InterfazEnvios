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

namespace DLLMT202
{
    public class Mt202
    {

        Configuracion config = new Configuracion();

        public enum IndexFormat
        {
            Format00 = 0,
            Format01 = 1,
            Format02 = 2,
            Format03 = 3,
            Format04 = 4,
            Format05 = 5,
            Format06 = 6,
            Format07 = 7,
            Format08 = 8,
            Format09 = 9,
            Format10 = 10,
            Format11 = 11,
            Format12 = 12,
            Format13 = 13,
            Format14 = 14,
            Format15 = 15,
            Format16 = 16,
            Format17 = 17
        }


        //private cnnConexion ADODB.Connection 'Objeto para conexion DB
        private string strSQL;           //Variabla para sentencias SQL
        private long lngError;           //Variable para no de error
        private string strErrorDescrip;  //Variable para desc. de error
        private string[] arrFormato;     //Arreglo para el formato de mensajes 'LEPR 13/09/2018
        private string[] arrMT202;       //Arreglo para guradar los numeros de reporte
        private bool blnDatosMT202;      //Si hay o no datos en Swift
        private bool blnPrimerRenglon;
        private bool blnUltimoRenglon;
        private string strBancoOrigen;
        private string strBancoDestino;
        private string strCampo52;
        private string strCampo53;
        private string strCampo57;
        private string strCampo58;
        private string strCampo72;
        bool banderaGPIMT202; //BPSA y LEPR, descripcion: Bandera para mostrar el campo 111 cuando su valor es True y cuando es False no se muestra. Estandares 2018
        string[] arrUETR;     //BPSA, descripcion: Arreglo para guardar UETR en base de datos. Estandares 2018
        string rutaUETR_TXT;  //LEPR ruta donde leera el UETR generado
        string rutaUETR_BAT;         //LEPR ruta donde ejecutara el Bat
        string logUETR;       //LEPR Muestra los generado por el UETR
        string rutaLog;       //LEPR Muestra la ruta del log
        string rutaUETR;      //LEPR Muestra la ruta donde se genera el UETR



        private void Sleep(int dwMilliseconds)
        {
            Thread.Sleep(dwMilliseconds);
        }

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

        private void Class_Initialize() 
        {
            IniciaArreglos();
            blnDatosMT202 = false;
        }
         
        public Boolean MT202Generation(string strFecha, int intAgencia1, 
                                        int intAgencia3, string strRutaMT202, 
                                        string strArchivoMT202, string cnnConection,
                                        int intTotRegs, string strAmbiente
                                        ) 
        {
            bool result = false;
            try
            {
                
                string strRutaArchivoMT202;
                int intArchivoMT202;
                int intLineaErrror;
                int intRegsMT202;
                //Dim rstInfoMT202        As New ADODB.Recordset
                List<REPORTE_SWIFT_MT202> rstMT202 = new List<REPORTE_SWIFT_MT202>();
                string strMensaje;
                int I;
                
                //BPSA y LEPR, descripcion: Obtiene el valor de la bandera desde el archivo InterfazEnvios.ini. Estandares 2018
                banderaGPIMT202 = bool.Parse(config.getValueAppConfig("bGPIMT202", "PARAMETROS"));
                
                intLineaErrror = 1;

                //Se crea una instancia de la conexión
                intLineaErrror = 2;
                //Verifica el formato de la ruta
                intLineaErrror = 3;
                if (strRutaMT202.Substring(strRutaMT202.Length -1,1).Trim() != @"\")
                {
                    strRutaMT202 = strRutaMT202.Trim() + @"\";
                }
                //Concatena la ruta y el nombre del archivo MT202
                intLineaErrror = 4;
                strRutaArchivoMT202 = strRutaMT202 + strArchivoMT202;
                //Cadena de instruccion para ejecutar el stored procedure
                //con sus respectivos parametros
                intLineaErrror = 5;
                //Abre Recordset ejecutando el stored procedure
                //que regresa dos recordsets
                intLineaErrror = 6;
                using (TICKETEntities contexto = new TICKETEntities())
                {
                    rstMT202 = (List<REPORTE_SWIFT_MT202>)(from R in contexto.REPORTE_SWIFT_MT202
                                      join B in contexto.BITACORA_ENVIO_SWIFT_MT202 on R.num_rep equals B.num_rep
                                      where R.fecha_reporte >= DateTime.ParseExact(strFecha, "mm/dd/yyyy", CultureInfo.InvariantCulture)
                                      && R.fecha_reporte <= DateTime.ParseExact(strFecha, "mm/dd/yyyy", CultureInfo.InvariantCulture)
                                      && R.agencia == intAgencia1 || R.agencia == intAgencia3
                                      && B.status_envio == 0
                                      orderby R.fecha_reporte
                                      select new List<REPORTE_SWIFT_MT202>());
                }
                //Verificamos que existan registros
                intLineaErrror = 7;
                intRegsMT202 = rstMT202.Count();
                //Abrimos el segundo recordset que es la informaci�n
                intLineaErrror = 8;
                //Verificamos que existan registros
                intLineaErrror = 9;
                if (rstMT202.Count() != 0)
                {
                    //Borra archivo SWIFT
                    intLineaErrror = 10;
                    BorrarArchivo(strRutaArchivoMT202);
                    //Abre archivo SWIFT
                    intLineaErrror = 11;
                    StreamWriter archivo2 = new StreamWriter(strRutaArchivoMT202);
                    //Borra arreglo y dimensiona
                    intLineaErrror = 12;
                    arrMT202 = new string[0];
                    arrUETR = new string[0]; //BPSA, descripcion: Elimina elementos del arreglo de UETR. Estandares 2018

                    blnPrimerRenglon = true;
                    int contador = 0;
                    foreach (REPORTE_SWIFT_MT202 item in rstMT202)
                    {

                        Array.Resize(ref arrMT202, contador);
                        Array.Resize(ref arrUETR, contador); //BPSA, descripcion: Se redimensiona arreglo de UETR. Estandares 2018
                        //Busca parametros segun ambiente
                        intLineaErrror = 13;
                        BuscarSegunAmbiente(strAmbiente, item.agencia, item.dep_mayor, item.ret_mayor);
                        //Escribe datos en el archivo MT202
                        intLineaErrror = 14;
                        //Arreglo que guarda el UETR
                        arrUETR[contador] = UETR(item.num_rep.ToString().Trim());
                        if (arrUETR[contador] == "")
                        {
                            //GoTo errArchivoPlano202
                            return result;
                        }

                        archivo2.WriteLine(ArchivoMT202(contador, item));
                        intLineaErrror = 15;
                        blnPrimerRenglon = false;
                        //Incrementa el tama�o del arreglo
                        intLineaErrror = 16;
                        if (contador > 0) Array.Resize(ref arrMT202, contador);

                        //Guarda datos en el arreglo
                        intLineaErrror = 17;
                        arrMT202[contador] = item.num_rep.ToString().Trim();

                        //Siguiente registro
                        intLineaErrror = 18;
                        contador++;
                    }

                    archivo2.WriteLine(arrFormato[17]);
                    //Cierra archivo SWIFT

                    intLineaErrror = 19;
                    archivo2.Close();

                    blnDatosMT202 = true;
                }
                else {
                    //No existen datos
                    intLineaErrror = 20;
                    blnDatosMT202 = false;   
                }
                intLineaErrror = 21;
                intLineaErrror = 22;

                /*-----------------------------------------------------------------------------------------------------
                BAPS y LEPR 29-08-2018 Se guarda la operacion y el UETR en base de datos de acuerdo al Estandar 2018
                -----------------------------------------------------------------------------------------------------*/
                bool bInsertaUETR;

                bInsertaUETR = insertaUETR();
                intLineaErrror = 23;
                intTotRegs = intRegsMT202;

                intLineaErrror = 24;
                if (blnDatosMT202)
                {
                    if (Actualizar(strArchivoMT202))
                    {
                        intLineaErrror = 25;
                        if (blnDatosMT202)
                        {
                            result = true;
                        }
                        else
                        {
                            intLineaErrror = 26;
                            result = false;
                            lngError = 0;
                            strErrorDescrip = "No existen datos.";
                        }
                    }
                    else 
                    {
                        result = true;
                    }
                }
                else {
                    result = true;
                }
                //Elimina de memoria los recordsets
                intLineaErrror = 27;
            }
            catch (Exception ex)
            {
                string message = "Error en el modulo MT202Generation libreria dll MT202 -> ";
                Log.Escribe(ex, message);
                result = false;
            }
            return result;
        }

        public long MT202Error => lngError;

        public string MT202ErrorDescripcion => strErrorDescrip;

        public bool MT202Status => blnDatosMT202;

        public void MT202ModificaFormato(IndexFormat Index, string Value) 
        {
            arrFormato[int.Parse(Index.ToString())] = Value;
        }

        private void IniciaArreglos() 
        {
            arrFormato[1] = "X";
            arrFormato[2] = "0000000000}";
            arrFormato[3] = "{2:I202";
            arrFormato[4] = "X";
            arrFormato[5] = "N}";
            arrFormato[6] = "{3:{108:T";
            arrFormato[7] = "}}";
            arrFormato[8] = "{4:";
            arrFormato[9] = ":20:";
            arrFormato[10] = ":21:";
            arrFormato[11] = ":32A:";
            arrFormato[12] = ":52A:";
            arrFormato[13] = ":53";
            arrFormato[14] = ":57";
            arrFormato[15] = ":58";
            arrFormato[16] = ":72:/BNF/";
            arrFormato[17] = "-}";
            //LEPR Se Agrega constantes para UETR
            //13-09-18
            arrFormato[18] = "}{111:001";
            arrFormato[19] = "}{121:";
        }

        private string ArchivoMT202(int contador, REPORTE_SWIFT_MT202 rstMT202)
        {
            string strMensaje = "";
            string strCampo32;

            if (blnPrimerRenglon)
            {
                strMensaje = arrFormato[0] + strBancoOrigen.Trim() + arrFormato[2];
                strMensaje = strMensaje + arrFormato[3] + strBancoDestino.Trim() + arrFormato[5];
                strMensaje = strMensaje + arrFormato[6] + DateTime.Now.ToString("yyyyMMdd") + "F2" + rstMT202.num_rep_202.ToString("0000000");
                //Bandera GPI False = No Muestra True = Muestra. Estandares 2018
                if (banderaGPIMT202)
                {
                    strMensaje = strMensaje + arrFormato[18];
                }
                strMensaje = strMensaje + arrFormato[19] + arrUETR[contador] + arrFormato[7];
                strMensaje = strMensaje + arrFormato[8] + Environment.NewLine;
            }
            else
            {
                strMensaje = arrFormato[17] + "$";
                strMensaje = strMensaje + arrFormato[0] + strBancoOrigen.Trim() + arrFormato[2];
                strMensaje = strMensaje + arrFormato[3] + strBancoDestino.Trim() + arrFormato[5];
                strMensaje = strMensaje + arrFormato[6] + DateTime.Now.ToString("yyyyMMdd") + "F2" + rstMT202.num_rep_202.ToString("0000000");
                //Bandera GPI False = No Muestra True = Muestra. Estandares 2018
                if (banderaGPIMT202)
                {
                    strMensaje = strMensaje + arrFormato[18];
                }
                strMensaje = strMensaje + arrFormato[19] + arrUETR[contador] + arrFormato[7];
                strMensaje = strMensaje + arrFormato[8] + Environment.NewLine;
            }

                strCampo32 = rstMT202.campo_32.Substring(0, 9).Trim() + rstMT202.campo_32.Trim().Substring(9).Replace(".", ",");
                strMensaje = strMensaje + arrFormato[9] + rstMT202.campo_20.Trim() + Environment.NewLine;
                strMensaje = strMensaje + arrFormato[10] + rstMT202.campo_21.Trim() + Environment.NewLine;
                strMensaje = strMensaje + arrFormato[11] + strCampo32 + Environment.NewLine;


                if (strCampo52.Trim().Length > 0)
                {
                    strMensaje = strMensaje + arrFormato[12] + strCampo52 + Environment.NewLine; 
                }
                if (strCampo53.Trim().Length > 0)
                {
                    strMensaje = strMensaje + arrFormato[13] + strCampo53 + Environment.NewLine;
                }
                if (strCampo57.Trim().Length > 0)
                {
                    strMensaje = strMensaje + arrFormato[14] + strCampo57 + Environment.NewLine;
                }
                if (strCampo58.Trim().Length > 0)
                {
                    strMensaje = strMensaje + arrFormato[15] + strCampo58 + Environment.NewLine;
                }
                if (strCampo72.Trim().Length > 0)
                {
                    strMensaje = strMensaje + arrFormato[16] + strCampo72;
                }

            //Debug.Print strMensaje
            return strMensaje;
        }

        private bool Actualizar(string strArchivoMT202) 
        {
            bool result = true;
            
            using (TICKETEntities contexto = new TICKETEntities()) 
            {
                using (var trans = contexto.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in arrMT202)
                        {
                            var reporte = contexto.REPORTE_SWIFT_MT202.SingleOrDefault(u => u.num_rep == int.Parse(item));
                            if (reporte != null)
                            {
                                reporte.status_reporte = 3;
                                contexto.SaveChanges();
                            }

                            var envio = contexto.BITACORA_ENVIO_SWIFT_MT202.SingleOrDefault(u => u.num_rep == int.Parse(item));
                            if (envio != null)
                            {
                                envio.fecha_envio = DateTime.Now;
                                envio.archivo = strArchivoMT202.Substring(0, strArchivoMT202.IndexOf('.')).Trim();
                                envio.status_envio = 1;
                                contexto.SaveChanges();
                            }
                        }

                        contexto.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        string message = "Error en el modulo Actualizar libreria dll MT202 -> ";
                        Log.Escribe(ex, message);
                        result = false;
                        trans.Rollback();
                    }
                }
            }
            return result;
        }

        private void BorrarArchivo(string strRutaNomArchivo) 
        {
            try
            {
                System.IO.File.Delete(strRutaNomArchivo);
            }
            catch (Exception ex)
            {
                string message = "Error en el modulo BorrarArchivo libreria dll MT202 -> ";
                Log.Escribe(ex, message);
            }
            
        }

        private void BuscarSegunAmbiente(string strAmbiente, int intAgencia, bool blnDepMayor, bool blnRetMayor) 
        {
            string[] arrValor;
            string strValor = "";
            List<string> lista = new List<string>();
            using (TICKETEntities contexto = new TICKETEntities())
            {
                if (strAmbiente.ToUpper() == "Producción".ToUpper())
                {
                    #region Agencia 1
                    if (intAgencia == 1)
                    {
                        #region blnRetMayor
                        if (blnRetMayor) 
                        {

                            #region ADDRESS_BRCH
                            //Banco Origen, Banco Destino, Campo52
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "ADDRESS_BRCH" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Banco Origen
                                strBancoOrigen = arrValor[0] + arrFormato[1] + arrValor[2];
                                //Banco Destino
                                strBancoDestino = arrValor[1] + arrFormato[4] + arrValor[3];
                                //Campo52
                                strCampo52 = arrValor[0];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region campo 53
                            //Campo53
                            strCampo53 = "D:/8000225300100" + Environment.NewLine + "FED";
                            #endregion

                            #region campo57
                            //Campo57
                            strCampo57 = "A:CHASUS33";
                            #endregion

                            #region BCOBENEFR1
                            //Campo58
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "BCOBENEFR1" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo58
                                strCampo58 = arrValor[0] + Environment.NewLine + arrValor[1];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region DATOSR1
                            //Campo72
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "DATOSR1" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo72
                                strCampo72 = arrValor[0] + Environment.NewLine + arrValor[1];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region LEYENDA1
                            //Campo72
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "LEYENDA1" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo72
                                strCampo72 = strCampo72 + arrValor[0];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion
                        }
                        #endregion
                        #region blnDepMayor
                        else if (blnDepMayor)
                        {
                            #region DATOSD1
                            //Banco Destino
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "DATOSD1" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Banco Destino
                                strBancoDestino = arrValor[0] + arrFormato[4];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region ADDRESS_BRCH
                            //Banco Origen, Banco Destino, Campo52
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "ADDRESS_BRCH" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Banco Origen
                                strBancoOrigen = arrValor[0] + arrFormato[1] + arrValor[2];
                                //Banco Destino
                                strBancoDestino = strBancoDestino + arrValor[3];
                                //Campo52
                                strCampo52 = arrValor[0];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region campo 53
                            //Campo53
                            strCampo53 = "A:/400001942" + Environment.NewLine + "BCMRMXMM";
                            #endregion

                            #region campo57
                            //Campo57
                            strCampo57 = "";
                            #endregion

                            #region BCOBENEFR1
                            //Campo58
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "BCOBENEFR1" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo58
                                strCampo58 = arrValor[0] + Environment.NewLine + arrValor[1];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region DATOSD1
                            //Campo72
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "DATOSD1" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo72
                                strCampo72 = arrValor[1] + "";
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region LEYENDA1
                            //Campo72
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "LEYENDA1" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo72
                                strCampo72 = strCampo72 + arrValor[0];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion


                        }
                        #endregion
                    }
                    #endregion
                    #region Agencia 3
                    else if (intAgencia == 3)
                    {
                        #region blnRetMayor
                        if (blnRetMayor)
                        {

                        }
                        #endregion
                        #region blnDepMayor
                        else if (blnDepMayor)
                        {
                            #region DATOSD3
                            //Banco Destino
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "DATOSD3" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Banco Destino
                                strBancoDestino = arrValor[0] + arrFormato[4];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region ADDRESS_BRCH
                            //Banco Origen, Banco Destino, Campo52
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "ADDRESS_BRCH" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Banco Origen
                                strBancoOrigen = arrValor[0] + arrFormato[1] + arrValor[2];
                                //Banco Destino
                                strBancoDestino = strBancoDestino + arrValor[3];
                                //Campo52
                                strCampo52 = arrValor[0];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region campo 53
                            //Campo53
                            strCampo53 = "A:/400001942" + Environment.NewLine + "BCMRMXMM";
                            #endregion

                            #region campo57
                            //Campo57
                            strCampo57 = "A:/021000021" + Environment.NewLine + "CHASUS33";
                            #endregion

                            #region BCOBENEFD3
                            //Campo58
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "BCOBENEFD3" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo58
                                strCampo58 = arrValor[0] + Environment.NewLine + arrValor[1] + Environment.NewLine + arrValor[2];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region DATOSD3
                            //Campo72
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "DATOSD3" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo72
                                strCampo72 = arrValor[1] + "";
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region LEYENDA3
                            //Campo72
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "LEYENDA3" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo72
                                strCampo72 = strCampo72 + arrValor[0];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion


                        }
                        #endregion
                    }
                    #endregion
                }
                else if (strAmbiente.ToUpper() == "Pruebas".ToUpper())
                {
                    #region Agencia 1
                    if (intAgencia == 1)
                    {
                        #region blnRetMayor
                        if (blnRetMayor)
                        {

                            #region ADDRESS_BRCH
                            //Banco Origen, Banco Destino, Campo52
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "ADDRESS_BRCH" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Banco Origen
                                strBancoOrigen = arrValor[0] + arrFormato[1] + arrValor[2];
                                //Banco Destino
                                strBancoDestino = arrValor[1] + arrFormato[4] + arrValor[3];
                                //Campo52
                                strCampo52 = arrValor[0];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region campo 53
                            //Campo53
                            strCampo53 = "D:/8000225300100" + Environment.NewLine + "FED";
                            #endregion

                            #region campo57
                            //Campo57
                            strCampo57 = "A:CHASUS33";
                            #endregion

                            #region BCOBENEFR1
                            //Campo58
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "BCOBENEFR1" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo58
                                strCampo58 = arrValor[0] + Environment.NewLine + arrValor[1];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region DATOSR1
                            //Campo72
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "DATOSR1" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo72
                                strCampo72 = arrValor[0] + Environment.NewLine + arrValor[1];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region LEYENDA1
                            //Campo72
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "LEYENDA1" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo72
                                strCampo72 = strCampo72 + arrValor[0];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion
                        }
                        #endregion
                        #region blnDepMayor
                        else if (blnDepMayor)
                        {
                            #region DATOSD1
                            //Banco Destino
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "DATOSD1" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Banco Destino
                                strBancoDestino = arrValor[0] + arrFormato[4];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region ADDRESS_BRCH
                            //Banco Origen, Banco Destino, Campo52
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "ADDRESS_BRCH" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Banco Origen
                                strBancoOrigen = arrValor[0] + arrFormato[1] + arrValor[2];
                                //Banco Destino
                                strBancoDestino = strBancoDestino + arrValor[3];
                                //Campo52
                                strCampo52 = arrValor[0];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region campo 53
                            //Campo53
                            strCampo53 = "A:/400001942" + Environment.NewLine + "BCMRMXMM";
                            #endregion

                            #region campo57
                            //Campo57
                            strCampo57 = "";
                            #endregion

                            #region BCOBENEFR1
                            //Campo58
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "BCOBENEFR1" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo58
                                strCampo58 = arrValor[0] + Environment.NewLine + arrValor[1];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region DATOSD1
                            //Campo72
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "DATOSD1" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo72
                                strCampo72 = arrValor[1] + "";
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region LEYENDA1
                            //Campo72
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "LEYENDA1" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo72
                                strCampo72 = strCampo72 + arrValor[0];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion


                        }
                        #endregion
                    }
                    #endregion
                    #region Agencia 3
                    else if (intAgencia == 3)
                    {
                        #region blnRetMayor
                        if (blnRetMayor)
                        {

                        }
                        #endregion
                        #region blnDepMayor
                        else if (blnDepMayor)
                        {
                            #region DATOSD3
                            //Banco Destino
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "DATOSD3" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Banco Destino
                                strBancoDestino = arrValor[0] + arrFormato[4];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region ADDRESS_BRCH
                            //Banco Origen, Banco Destino, Campo52
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "ADDRESS_BRCH" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Banco Origen
                                strBancoOrigen = arrValor[0] + arrFormato[1] + arrValor[2];
                                //Banco Destino
                                strBancoDestino = strBancoDestino + arrValor[3];
                                //Campo52
                                strCampo52 = arrValor[0];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region campo 53
                            //Campo53
                            strCampo53 = "A:/400001942" + Environment.NewLine + "BCMRMXMM";
                            #endregion

                            #region campo57
                            //Campo57
                            strCampo57 = "A:/021000021" + Environment.NewLine + "CHASUS33";
                            #endregion

                            #region BCOBENEFD3
                            //Campo58
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "BCOBENEFD3" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo58
                                strCampo58 = arrValor[0] + Environment.NewLine + arrValor[1] + Environment.NewLine + arrValor[2];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region DATOSD3
                            //Campo72
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "DATOSD3" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo72
                                strCampo72 = arrValor[1] + "";
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion

                            #region LEYENDA3
                            //Campo72
                            lista = (from p in contexto.PARAMETRIZACION where p.codigo == "LEYENDA3" select p.valor).ToList();
                            strValor = Valores(lista);
                            if (strValor.Length > 0)
                            {
                                arrValor = strValor.Split(',');
                                //Campo72
                                strCampo72 = strCampo72 + arrValor[0];
                            }
                            arrValor = new string[0];
                            lista.Clear();
                            #endregion


                        }
                        #endregion
                    }
                    #endregion
                }
            }

        }

        private string Valores(List<string> Lista) 
        {
            string result = "";
            foreach (var item in Lista)
            {
                result = result + item.Trim();
            }
            return result;
        }

        private string UETR(string soperacion) 
        {
            /*---------------------------------------------------------------------------------------------------- -
             BAPS y LEPR 28 - 09 - 2018 Se genera UETR por medio del archivo bach y lo deposita en un txt de paso, para el campo 121 del mensaje de acuerdo al Estandar 2018
            --------------------------------------------------------------------------------------------------- -

            'Instruccion que ejecuta el bat. Estadares 2018
            'Nuestro bat recibe 2 parametros (soperacion & " 2"):
            'soperacion = n�mero de reporte obtenida del recordset
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

            //Lee variable UETR del UETR202.txt (txt de paso) que genero el .bat. Estadares 2018
            StrRuta = rutaUETR_TXT + @"\UETR202.txt"; //"C:\UETR\UETR_TXT\UETR202.txt"

            // Crear un objeto de FileSystemInfo
            FileSystemInfo o_Fso = new FileInfo(StrRuta);

            //Borra el contenido del archivo
            StreamWriter f1 = new StreamWriter(StrRuta);

            //"y finalmente con esto cierras el archivo"
            f1.Close();

            //res = ShellExecute(0, "", "C:\UETR\UETRV4_BAT.bat ", soperacion & " 2", "", 0)
            res = ShellExecute(0, "", rutaUETR_BAT + "\\UETRV4_BAT.bat", soperacion + " 2 " + logUETR + " " + rutaLog + " " + rutaUETR, "", 0);

            Sleep(1500);

            // Abrir el archivo en modo de escritura
            StreamReader archivo = new StreamReader(StrRuta);

            // Leer el archivo en un bucle hasta que se cumpla la condición
            do
            {
                contador++;

                if (contador == 100)
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
                        foreach (var item in arrMT202)
                        {
                            contexto.TICKET_UETR.Add(
                                new TICKET_UETR { 
                                    operacion = int.Parse(arrMT202[cont]), 
                                    UETR = arrUETR[cont], 
                                    fecha_actual = DateTime.Now, 
                                    usuario = "InterfazEnvios" });
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


    }
}
