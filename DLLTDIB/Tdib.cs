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
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;

namespace DLLTDIB
{
    public class Tdib
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

        private string cnnConexion;// As ADODB.Connection 'Objeto para conexion DB
        private string strsql; //             As String           'Variabla para sentencias SQL
        private long lngError; //           As Long             'Variable para no de error
        private string strErrorDescrip;//     As String           'Variable para desc. de error
        private string[] arrFormato;//      As String           'Arreglo para el formato de mensajes
        private bool blnDatosEQT;//         As Boolean          'Si hay o no datos en Equation
        private bool blnDatosSWF;//         As Boolean          'Si hay o no datos en Swift
        private string strRetiros;//          As String           'Complemento del formato de swift retiros
        private string strDepositos;//        As String           'Complemento del formato de swift depositos
        private string strBasicHBranch;//    As String            'Complemento del formato de swift retiros
        private string strAplicationHBranch;//  As String         'Complemento del formato de swift depositos
        private bool blnPrimerRenglon;//    As Boolean
        private bool blnUltimoRenglon;//    As Boolean
        private string[] arrTDIB1;//          As String           'CHDG
        private string[] arrTDIB2;//          As String           'CHDG

        private void Class_Initialize() 
        {
            blnDatosEQT = false;
            blnDatosSWF = false;
        }

        public bool TDIBGeneration(string strFecha, int intAgencia, string strRutaTDIB, string strRutaSWIFT, 
                                    string strArchivoTDIB, string strArchivoSWIFT, string strAttnNombre, 
                                    bool blnArchivoEQT, bool blnArchivoSWF, string cnnConection, string strAmbiente, int varTotRegs) 
        {
            bool result = false;

            string strRutaArchivoTDIB;
            string strRutaArchivoSWIFT;
            int intArchivoTDIB = 0;
            int intArchivoSWIFT = 0;
            int intLineaErrror;
            int intRegsEQT;
            int intRegsSWF;
            int intTotRegs;
            //Dim rstInfoTDIB;
            //List<GTDIB2> rstEquation;
            //List<GTDIB2> rstSwift;
            string strMensaje;
            int I;
            int[] arrTotRegs;

            try
            {
                intLineaErrror = 1;
                intLineaErrror = 2;

                //Verifica el formato de la ruta
                if (strRutaTDIB.Substring(strRutaTDIB.Length - 1, 1).Trim() != @"\")
                {
                    strRutaTDIB = strRutaTDIB.Trim() + @"\";
                }
                if (strRutaSWIFT.Substring(strRutaSWIFT.Length - 1, 1).Trim() != @"\")
                {
                    strRutaSWIFT = strRutaSWIFT.Trim() + @"\";
                }

                intLineaErrror = 3;
                //Concatena la ruta y el nombre del archivo TDIB
                strRutaArchivoTDIB = strRutaTDIB + strArchivoTDIB;
                //Concatena la ruta y el nombre del archivo SWIFT
                strRutaArchivoSWIFT = strRutaSWIFT + strArchivoSWIFT;

                intLineaErrror = 4;
                //Validamos opciones de archivos
                if (blnArchivoEQT && blnArchivoSWF)
                {
                    intAgencia = intAgencia;
                }
                else if (blnArchivoEQT && !blnArchivoSWF)
                {
                        intAgencia = intAgencia;
                }
                else if (!blnArchivoEQT && blnArchivoSWF)
                {
                    intAgencia = 1;
                }

                intLineaErrror = 5;
                if (blnArchivoEQT) 
                {
                    intArchivoTDIB = 1; 
                }
                if (blnArchivoSWF) 
                {
                    intArchivoSWIFT = 1; 
                }

                intLineaErrror = 6;
                intLineaErrror = 7;
                /*Cadena de instruccion para ejecutar el stored procedure
                con sus respectivos parametros
                            strsql = "Exec TICKET..sp_i_InfoTDIB "
                            strsql = strsql & "'" & strFecha & "', " & intAgencia & ", "
                            strsql = strsql & intArchivoTDIB & ", " & intArchivoSWIFT*/


                intLineaErrror = 8;

                using (TICKETEntities contexto = new TICKETEntities())
                {
                    /*var rstInfoTDIB = contexto.Database.SqlQuery<MultipleRecordsetResult<List<GTDIB2>,List<GTDIB2>>>($@"Exec TICKET..sp_i_InfoTDIB ", 
                        new SqlParameter("@fechaoperacion", strFecha), 
                        new SqlParameter("@agencia", intAgencia), 
                        new SqlParameter("@equation", intArchivoTDIB),
                        new SqlParameter("@swift", intArchivoSWIFT));*/

                    var cmd = contexto.Database.Connection.CreateCommand();
                    cmd.CommandText = "[dbo].[sp_i_InfoTDIB]";
                    EntityParameter param1 = new EntityParameter();
                    param1.ParameterName = "fechaoperacion";
                    param1.Value = strFecha;
                    cmd.Parameters.Add(param1);
                    EntityParameter param2 = new EntityParameter();
                    param2.ParameterName = "agencia";
                    param2.Value = intAgencia;
                    cmd.Parameters.Add(param2);
                    EntityParameter param3 = new EntityParameter();
                    param3.ParameterName = "equation";
                    param3.Value = intArchivoTDIB;
                    cmd.Parameters.Add(param3);
                    EntityParameter param4 = new EntityParameter();
                    param4.ParameterName = "swift";
                    param4.Value = intArchivoSWIFT;
                    cmd.Parameters.Add(param4);

                    contexto.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();


                    /**************************************************
                    VALIDAMOS SI REQUIEREN EL ARCHIVO EQUATION
                    **************************************************/
                    if (blnArchivoEQT)
                    {
                        intLineaErrror = 9;
                        //Abrimos el primer recordset que es la informaci�n de equation
                        var rstEquation = ((IObjectContextAdapter)contexto)
                                .ObjectContext
                                .Translate<GTDIB2>(reader, "GTDIB2", MergeOption.AppendOnly);
                        intLineaErrror = 10;
                        //Si existen datos
                        if (rstEquation.Count() > 0)
                        {
                            intLineaErrror = 11;
                            //Borra archivo TDIB
                            BorrarArchivo(strRutaArchivoTDIB);

                            intLineaErrror = 12;
                            //Abre archivo TDIB



                            foreach (var item in rstEquation)
                            {

                            }
                        }
                    }



                    /**************************************************
                    VALIDAMOS SI REQUIEREN EL ARCHIVO SWIFT
                    **************************************************/
                    if (blnArchivoSWF)
                    {
                        intLineaErrror = 25;
                        //Abrimos el segundo recordset que es la información de swift
                        reader.NextResult();
                        var rstSwift = ((IObjectContextAdapter)contexto)
                                .ObjectContext
                                .Translate<GTDIB2>(reader, "GTDIB2", MergeOption.AppendOnly);
                        intLineaErrror = 26;
                        //Si existen datos
                        if (rstSwift.Count() > 0)
                        {
                            foreach (var item in rstSwift)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = "Error en el modulo TDIBGeneration libreria dll Tdib -> ";
                Log.Escribe(ex, message);
            }





            return result;
        }

        public long TDIBError => lngError;

        public string TDIBErrorDescripcion => strErrorDescrip;

        public bool TDIBEquationStatus => blnDatosEQT;

        public bool TDIBSwiftStatus => blnDatosSWF;

        public void TDIBModificaFormato(IndexFormat Index, string Value) 
        {
            arrFormato[int.Parse(Index.ToString())] = Value;
        }

        private void BuscarSegunAmbiente(string strAmbiente) 
        {
            string[] arrValor;
            string codigoABucar = "";

            arrValor = new string[0];

            /*Tabla de PARAMETRIZACION en BD DESARROLLO
                valor                     codigo        descripcion
                BCMRMXMM,BCMRUS4H,OPE,XXX ADDRESS_BRCH  Basic & Application Headers&Branches - Produccion
                BCMRMXM0,BCMRMXM0,OPE,OPE ADDRESS_BRCHP Basic & Application Headers&Branches - Pruebas*/

            try
            {
                if (string.Compare(strAmbiente, "Pruebas", StringComparison.OrdinalIgnoreCase) == 0) {
                    codigoABucar = "ADDRESS_BRCHP";
                }
                else if (string.Compare(strAmbiente, "Producción", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    codigoABucar = "ADDRESS_BRCH";
                }

                using (TICKETEntities contexto = new TICKETEntities())
                {
                    string rstTemp = (from p in contexto.PARAMETRIZACION where p.codigo == codigoABucar select p.valor).FirstOrDefault();

                    if (rstTemp.Length > 0)
                    {
                        arrValor = rstTemp.Split(',');
                        strRetiros = arrValor[0].Trim();
                        strDepositos = arrValor[1].Trim();
                        strBasicHBranch = arrValor[2].Trim();
                        strAplicationHBranch = arrValor[3].Trim();
                    }
                    else 
                    {
                        strRetiros = "";
                        strDepositos = "";
                        strBasicHBranch = "";
                        strAplicationHBranch = "";
                    }
                }
            }
            catch (Exception ex)
            {
                string message = "Error en el modulo TDIBGeneration libreria dll Tdib -> ";
                Log.Escribe(ex, message);
            }
        }
        /*
        ' Procedimiento:Para iniciar el arreglo que es el formato del mensaje swift.
        ' Parametros:   Ninguno.
        ' Retorno:      Ninguno.
        ' Nota:         Se manda a llamar una sola vez cuando se iniciliza la clase.
        */
        private void IniciaArreglos() 
        {
            arrFormato[0] = "{1:F01";
            //OLIVIA FARIAS 28/SEP/21
            //SE REALIZA CAMBIO DE PARA EL INDICADOR DE LOS MENSAJES SWIFT DE LOGICAL TERMINAL DE A POR X
            //arrFormato[1] = "A" & strBasicHBranch + "0000000000}"
            arrFormato[1] = "X" + strBasicHBranch + "0000000000}";  //OFG
            arrFormato[2] = "{2:I198";
            arrFormato[3] = "N}";
            arrFormato[4] = "{3:{108:T";
            arrFormato[5] = "{4:";
            arrFormato[6] = ":20:";
            arrFormato[7] = ":12:198";
            arrFormato[8] = ":77E:";
            arrFormato[9] = "Attn. ";
            arrFormato[10] = "T I M E  D E P O S I T";
            arrFormato[11] = "Customer name.:";
            arrFormato[12] = "Deal Type. . .:";
            arrFormato[13] = "Reference. . .:";
            arrFormato[14] = "Account. . . .:";
            arrFormato[15] = "Amount . . . .:";
            arrFormato[16] = "Rate . . . . .:";
            arrFormato[17] = "Days . . . . .:";
            arrFormato[18] = "Starting Date.:";
            arrFormato[19] = "Maturity Date.:";
            arrFormato[20] = "Attn.";
            arrFormato[21] = "MESA NEGOCIOS INTERNACIONALES, MNI";
            arrFormato[22] = "PLEASE, REPORT RECEPTION OF SWIFT";
            arrFormato[23] = "TELEPHONE MEXICO 525(621 26 60)";
            arrFormato[24] = "-}";
            arrFormato[25] = "X" + strAplicationHBranch;
        }

        private string ArchivoTDIB(string rstRecordSet) 
        { 
            string result = "";
            return result;
        }

        private string ArchivoSWIFT(string rstRecordSet) 
        {
            string result = "";
            return result;
        }

        private void StatusOperacion(long lngOperacion) 
        { 
            
        }

        private void BitacoraEnvioKapiti(long lngOperacion, string strArchivo) 
        { 
            
        }

        private void BitacoraEnvioSWIFT(long lngOperacion, string strArchivo) 
        { 
            
        }

        private void BitacoraEnvios(string strArchivo) 
        {
            try
            {
                    ModeloNegocio.Transferencias.FibipcSave(strArchivo,DateTime.Now);
            }
            catch (Exception ex)
            {
                string message = "Error en el modulo BorrarArchivo libreria dll MT202 -> ";
                Log.Escribe(ex, message);
            }
        }

        private bool Actualizar(bool bEquation, bool bSwift, string strArchivoTDIB, string strArchivoSWIFT) 
        {
            bool result = false;
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

        private string FillText(string sTexto, int intNumero, string strRellenar, bool blnDerecha)
        {
            string result;
            string trimmedText = sTexto.Trim();

            if (trimmedText.Length < intNumero)
            {
                if (blnDerecha)
                {
                    result = trimmedText + new string(strRellenar[0], intNumero - trimmedText.Length);
                }
                else
                {
                    result = new string(strRellenar[0], intNumero - trimmedText.Length) + trimmedText;
                }
            }
            else
            {
                result = trimmedText;
            }
            return result.Substring(0, intNumero);
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

        public class MultipleRecordsetResult<T1, T2>
        {
            public List<T1> ResultSet1 { get; set; }
            public List<T2> ResultSet2 { get; set; }
        }
    }
}
