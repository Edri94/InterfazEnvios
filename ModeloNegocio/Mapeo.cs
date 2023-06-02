using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ModeloNegocio
{
    public class Mapeo
    {
        #region DLLImport
        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2A(ref NETRESOURCE lpNetResource, string lpPassword, string lpUsername, int dwFlags);

        [StructLayout(LayoutKind.Sequential)]
        private class NETRESOURCE
        {
            public int dwScope;
            public int dwType;
            public int dwDisplayType;
            public int dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }
        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(
        NETRESOURCE lpNetResource,
        string lpPassword,
        string lpUserName,
        int dwFlags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection(
            string lpszName,
            int bForce);

        [DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int WNetAddConnection2(ref NETRESOURCE lpNetResource, string lpPassword, string lpUsername, int dwFlags);

        [DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int WNetCancelConnection2(string lpName, int dwFlags, bool fForce);
        #endregion

        #region Constantes


        private const int NO_ERROR = 0;
        private const int CONNECT_UPDATE_PROFILE = 0x1;
        private const int RESOURCETYPE_DISK = 0x1;
        private const int RESOURCETYPE_PRINT = 0x2;
        private const int RESOURCETYPE_ANY = 0x0;
        private const int RESOURCE_CONNECTED = 0x1;
        private const int RESOURCE_REMEMBERED = 0x3;
        private const int RESOURCE_GLOBALNET = 0x2;
        private const int RESOURCEDISPLAYTYPE_DOMAIN = 0x1;
        private const int RESOURCEDISPLAYTYPE_GENERIC = 0x0;
        private const int RESOURCEDISPLAYTYPE_SERVER = 0x2;
        private const int RESOURCEDISPLAYTYPE_SHARE = 0x3;
        private const int RESOURCEUSAGE_CONNECTABLE = 0x1;
        private const int RESOURCEUSAGE_CONTAINER = 0x2;

        // Constantes de error:
        private const int ERROR_BAD_USERNAME = 2202;
        private const int ERROR_NOT_CONNECTED = 2250;
        private const int ERROR_OPEN_FILES = 2401;
        private const int ERROR_DEVICE_IN_USE = 2404;
        private const int ERROR_BAD_DEVICE = 1200;
        private const int ERROR_CONNECTION_UNAVAIL = 1201;
        private const int ERROR_DEVICE_ALREADY_REMEMBERED = 1202;
        private const int ERROR_NO_NET_OR_BAD_PATH = 1203;
        private const int ERROR_BAD_PROVIDER = 1204;
        private const int ERROR_CANNOT_OPEN_PROFILE = 1205;
        private const int ERROR_BAD_PROFILE = 1206;
        private const int ERROR_NOT_CONTAINER = 1207;
        private const int ERROR_EXTENDED_ERROR = 1208;
        private const int ERROR_INVALID_GROUPNAME = 1209;
        private const int ERROR_INVALID_COMPUTERNAME = 1210;
        private const int ERROR_INVALID_EVENTNAME = 1211;
        private const int ERROR_INVALID_DOMAINNAME = 1212;
        private const int ERROR_INVALID_SERVICENAME = 1213;
        private const int ERROR_INVALID_NETNAME = 1214;
        private const int ERROR_INVALID_SHARENAME = 1215;
        private const int ERROR_INVALID_PASSWORDNAME = 1216;
        private const int ERROR_INVALID_MESSAGENAME = 1217;
        private const int ERROR_INVALID_MESSAGEDEST = 1218;
        private const int ERROR_SESSION_CREDENTIAL_CONFLICT = 1219;
        private const int ERROR_REMOTE_SESSION_LIMIT_EXCEEDED = 1220;
        private const int ERROR_DUP_DOMAINNAME = 1221;
        private const int ERROR_NO_NETWORK = 1222;
        private const int ERROR_ACCESS_DENIED = 5;
        private const int ERROR_ALREADY_ASSIGNED = 85;
        private const int ERROR_BAD_DEV_TYPE = 66;
        private const int ERROR_BAD_NET_NAME = 67;
        private const int ERROR_BUSY = 170;
        private const int ERROR_CANCELLED = 1223;
        private const int ERROR_INVALID_PASSWORD = 86;

        // Variable numerica para guardar el no. de error
        public static int Mn_Error;

        // Variable caracter para guardar la descripcion de error
        private static string Ms_Error;

        // Variable numerica para guardar el no. de error
        private static int Mn_ErrorTest;

        // Variable caracter para guardar la descripcion de error
        private static string Ms_ErrorTest;

        private static string Ms_ErrorTipo;

        // Variables para el regreso
        private static string[] arrArchivos; // Variable arreglo para recorrer los archivos del Filelist
        private static string[] arrArchivosOK; // Variable arreglo para guardar los archivos copiados
        private static bool blnResultado; // Variable booleana para determinar si se copió por lo menos un archivo
        public static int lngErrorReg;
        public static string strErrorReg;

        public static int DriveError
        {
            get { return Mn_Error; }
        }
        public static int ArchivoError
        {
            get { return Mn_Error; }
        }

        public static string DriveErrorDescripcion
        {
            get
            {
                switch (Mn_Error)
                {
                    case ERROR_BAD_USERNAME: // 2202
                        return "Bad user name.";
                    case ERROR_NOT_CONNECTED: // 2250
                        return "This network connection does not exist.";
                    case ERROR_OPEN_FILES: // 2401
                        return "This network connection has files open or requests pending.";
                    case ERROR_DEVICE_IN_USE: // 2404
                        return "The device is in use by an active process and cannot be disconnected.";
                    case ERROR_BAD_DEVICE: // 1200
                        return "The specified device name is invalid.";
                    case ERROR_CONNECTION_UNAVAIL: // 1201
                        return "The device is not currently connected but it is a remembered connection.";
                    case ERROR_DEVICE_ALREADY_REMEMBERED: // 1202
                        return "An attempt was made to remember a device that had previously been remembered.";
                    case ERROR_NO_NET_OR_BAD_PATH: // 1203
                        return "No network provider accepted the given network path.";
                    case ERROR_BAD_PROVIDER: // 1204
                        return "The specified network provider name is invalid.";
                    case ERROR_CANNOT_OPEN_PROFILE: // 1205
                        return "Unable to open the network connection profile.";
                    case ERROR_BAD_PROFILE: // 1206
                        return "The network connection profile is corrupt.";
                    case ERROR_NOT_CONTAINER: // 1207
                        return "Cannot enumerate a non-container.";
                    case ERROR_EXTENDED_ERROR: // 1208
                        return "An extended error has occurred.";
                    case ERROR_INVALID_GROUPNAME: // 1209
                        return "The format of the specified group name is invalid.";
                    case ERROR_INVALID_COMPUTERNAME: // 1210
                        return "The format of the specified computer name is invalid.";
                    case ERROR_INVALID_EVENTNAME: // 1211
                        return "The format of the specified event name is invalid.";
                    case ERROR_INVALID_DOMAINNAME: // 1212
                        return "The format of the specified domain name is invalid.";
                    case ERROR_INVALID_SERVICENAME: // 1213
                        return "The format of the specified service name is invalid.";
                    case ERROR_INVALID_NETNAME: // 1214
                        return "The format of the specified network name is invalid.";
                    case ERROR_INVALID_SHARENAME: // 1215
                        return "The format of the specified share name is invalid.";
                    case ERROR_INVALID_PASSWORDNAME: // 1216
                        return "The format of the specified password is invalid.";
                    case ERROR_INVALID_MESSAGENAME: // 1217
                        return "The format of the specified message name is invalid.";
                    case ERROR_INVALID_MESSAGEDEST: // 1218
                        return "The format of the specified message destination is invalid.";
                    case ERROR_SESSION_CREDENTIAL_CONFLICT: // 1219
                        return "The credentials supplied conflict with an existing set of credentials.";
                    case ERROR_REMOTE_SESSION_LIMIT_EXCEEDED: // 1220
                        return "An attempt was made to establish a session to a Lan Manager server, but there are already too many sessions established to that server.";
                    case ERROR_DUP_DOMAINNAME: // 1221
                        return "The workgroup or domain name is already in use by another computer on the network.";
                    case ERROR_NO_NETWORK: // 1222
                        return "The network is not present or not started.";
                    case ERROR_ACCESS_DENIED: // 5
                        return "Access denied.";
                    case ERROR_ALREADY_ASSIGNED: //85
                        return "Device already assigned.";
                    case ERROR_BAD_DEV_TYPE: //66
                        return "Bad device type.";
                    case ERROR_BAD_NET_NAME: //67
                        return "Bad network name.";
                    case ERROR_BUSY: //170
                        return "Busy.";
                    case ERROR_CANCELLED: //1223
                        return "Cancelled.";
                    case ERROR_INVALID_PASSWORD: //86
                        return "Invalid password.";
                    default:
                        return "Error no detectado.";
                }
            }

        }

        public static string ArchivoErrorDescripcion
        {
            get
            {
                // Validate the error number and return the description
                switch (Mn_Error)
                {
                    case 52:
                        return "Nombre o número de archivo incorrecto.";
                    case 53:
                        return "No se pudo encontrar el archivo especificado.";
                    case 54:
                        return "Modo de archivo incorrecto.";
                    case 55:
                        return "El archivo ya está abierto.";
                    case 57:
                        return "Error de E/S de dispositivo.";
                    case 58:
                        return "El archivo ya existe.";
                    case 61:
                        return "Disco lleno.";
                    case 67:
                        return "Hay demasiados archivos.";
                    case 68:
                        return "El dispositivo no está disponible.";
                    case 70:
                        return "Se ha denegado el permiso.";
                    case 71:
                        return "El disco no está listo.";
                    case 74:
                        return "No se pudo cambiar el nombre con una unidad de disco diferente.";
                    case 75:
                        return "Error de acceso a la ruta o al archivo.";
                    case 76:
                        return "No se ha encontrado la ruta de acceso.";
                    default:
                        return Ms_Error;
                }
            }
        }

        public static long MapeoError
        {
            get
            {
                return Mn_ErrorTest;
            }
        }

        public static string MapeoErrorDescripcion
        {
            get
            {
                return Ms_ErrorTest;
            }
        }

        public static string MapeoErrorTipo
        {
            get
            {
                return Ms_ErrorTipo;
            }
        }

        public static long RegresoError
        {
            get { return lngErrorReg; }
        }

        // Propiedad: Regresa la descripcion de error del regreso
        public static string RegresoErrorDescripcion
        {
            get { return strErrorReg; }
        }

        // Propiedad para obtener la lista de los archivos copiados
        public static object RegresoListaArchivos
        {
            get { return arrArchivosOK; }
        }

        // Propiedad para obtener si existe por lo menos un archivo copiado
        public static bool RegresoListaOK
        {
            get { return blnResultado; }
        }
        #endregion
        private static bool DriveConectar(string Ls_RutaRemota, string Ls_UnidadLocal, string Ls_User = "", string Ls_Password = "")
        {
            NETRESOURCE NETR = new NETRESOURCE();
            int ErrInfo = 0;

            // Cambiar el cursor a modo espera
            // Cursor.Current = Cursors.WaitCursor;

            ErrInfo = 0;
            // Asignacion de valores hacia la variable definida por usuario
            NETR.dwScope = RESOURCE_GLOBALNET;
            NETR.dwType = RESOURCETYPE_DISK;
            NETR.dwDisplayType = RESOURCEDISPLAYTYPE_SHARE;
            NETR.dwUsage = RESOURCEUSAGE_CONNECTABLE;
            NETR.lpRemoteName = Ls_RutaRemota;
            NETR.lpLocalName = Ls_UnidadLocal;

            Mn_Error = 0;
            // Genera el mapeo de la unidad por medio de la api mpr.dll
            ErrInfo = WNetAddConnection2(NETR, Ls_Password, Ls_User, CONNECT_UPDATE_PROFILE);
            Mn_Error = ErrInfo;
            // Cambiar el cursor de vuelta al modo predeterminado
            // Cursor.Current = Cursors.Default;
            return ErrInfo == NO_ERROR;
        }

        private static void DriveDesconectar(string Ls_UnidadLocal)
        {
            try
            {
                // Desconecta el mapeo de la unidad por medio de la api mpr.dll
                int result = WNetCancelConnection(Ls_UnidadLocal + '\0', 1);
                Mn_Error = result;
                if (result != 0) Console.WriteLine("Error DisconnectDrive Error:" + result.ToString());
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
            }
        }

        public static bool ArchivoBuscar(string Ls_RutaArchivo)
        {
            try
            {
                // Función que regresa el atributo de un archivo o carpeta,
                // se utiliza para saber si existe, si no existe se genera
                // un error y por lo tanto se simula la búsqueda del archivo.
                File.GetAttributes(Ls_RutaArchivo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ArchivoCopiar(string Ls_Origen, string Ls_Destino, bool Lb_ReemplazarArchivo = true)
        {
            try
            {
                //Cursor.Current = Cursors.WaitCursor;

                Mn_Error = 0;
                Ms_Error = "";

                // Verifica si existe el archivo destino
                if (ArchivoBuscar(Ls_Destino))
                {
                    // Si se reemplaza archivo
                    if (Lb_ReemplazarArchivo)
                    {
                        // Copia un archivo del origen al destino
                        File.Copy(Ls_Origen, Ls_Destino, true);
                        return true;
                    }
                    else
                    {
                        // Si no se reemplaza genera un error
                        Mn_Error = 58;
                        Ms_Error = "El archivo ya existe.";
                        return false;
                    }
                }
                else
                {
                    // Copia un archivo del origen al destino
                    File.Copy(Ls_Origen, Ls_Destino, true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Mn_Error = ex.HResult;
                Ms_Error = ex.Message;
                return false;
            }
            finally
            {
                //Cursor.Current = Cursors.Default;
            }
        }

        public bool Regreso(string strFiltroArchivos, string strRutaOrigen, string strRutaDestino, string strRutaRemota, string strUnidadLocal, string strUser = "", string strPassword = "")
        {
            //Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (RegresoCopiar(strFiltroArchivos, strRutaOrigen, strRutaDestino))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                lngErrorReg = ex.HResult;
                strErrorReg = ex.Message;
                //Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private bool RegresoCopiar(string strFiltroArchivos, string strRutaOrigen, string strRutaDestino)
        {
            try
            {
                blnResultado = false;

                arrArchivos = BuscarArchivos(strFiltroArchivos, strRutaOrigen);
                if (arrArchivos != null)
                {
                    int J = 0;
                    Array.Resize(ref arrArchivosOK, J);
                    blnResultado = false;

                    for (int I = 0; I < arrArchivos.Length; I++)
                    {
                        string strArchivo1 = arrArchivos[I];
                        string strArchivo2 = Path.Combine(strRutaDestino, Path.GetFileName(arrArchivos[I]));

                        if (ArchivoCopiar(strArchivo1, strArchivo2))
                        {
                            if (J > 0) Array.Resize(ref arrArchivosOK, J);
                            arrArchivosOK[I] = arrArchivos[I];
                            J = J + 1;
                            blnResultado = true;
                        }
                    }
                }
                else
                {
                    lngErrorReg = -1;
                    strErrorReg = "No existen archivos en la ruta: " + strRutaOrigen + " con el filtro: " + strFiltroArchivos;
                }

                if (blnResultado)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                lngErrorReg = ex.HResult;
                strErrorReg = ex.Message;
                return false;
            }
        }

        public static string[] BuscarArchivos(string filtro, string rutaOrigen)
        {
            var archivos = Directory.GetFiles(rutaOrigen, filtro);

            if (archivos.Length == 0)
            {
                lngErrorReg = -1;
                strErrorReg = "No se encontraron archivos con el filtro especificado.";
                return null;
            }

            return archivos;
        }

        private static bool ArchivoCopiar(string strArchivo1, string strArchivo2)
        {
            try
            {
                File.Copy(strArchivo1, strArchivo2, true);
                return true;
            }
            catch (Exception ex)
            {
                lngErrorReg = ex.HResult;
                strErrorReg = ex.Message;
                return false;
            }
        }

        public static System.Collections.Generic.List<string> ListarArchivos(string ruta, string filtro)
        {
            /*
            List<string> archivos = Archivo.ListarArchivos("C:\\Archivos", "*.txt");

               if (archivos != null)
               {
                   foreach (string archivo in archivos)
                   {
                       Console.WriteLine(archivo);
                   }
               }
               else
               {
                   Console.WriteLine("Error al acceder a los archivos.");
               }
            */
            System.Collections.Generic.List<string> archivos = new System.Collections.Generic.List<string>();

            try
            {
                archivos.AddRange(Directory.GetFiles(ruta, filtro));

                foreach (string subruta in Directory.GetDirectories(ruta))
                {
                    archivos.AddRange(ListarArchivos(subruta, filtro));
                }

                return archivos;
            }
            catch (IOException ex)
            {
                return null;
            }
        }

        private static bool Test(string archivoOrigen, string archivoDestino, string rutaRemota, string unidadLocal, string usuario = "", string contraseña = "", bool reemplazarArchivo = true, bool desconectarUnidad = false)
        {


            bool resultado = Mapear(archivoOrigen, archivoDestino, rutaRemota, unidadLocal, usuario, contraseña, reemplazarArchivo, desconectarUnidad);

            if (resultado)
            {
                Console.WriteLine("El archivo fue copiado exitosamente.");
            }
            else
            {
                Console.WriteLine("Ocurrió un error al copiar el archivo.");
                Console.WriteLine($"Error: {ArchivoError} - {ArchivoErrorDescripcion}");
            }
            return resultado;
        }

        public static bool Mapear(string archivoOrigen, string archivoDestino, string rutaRemota, string unidadLocal, string usuario = "", string contraseña = "", bool reemplazarArchivo = true, bool desconectarUnidad = false)
        {
            try
            {
                // Intenta conectar la unidad de red (mapeo).
                if (DriveConectar(rutaRemota, unidadLocal, usuario, contraseña))
                {
                    // Intenta copiar el archivo especificado hacia la unidad que se mapeó.
                    if (ArchivoCopiar(archivoOrigen, archivoDestino, reemplazarArchivo))
                    {
                        return true;
                    }
                    else
                    {
                        // Si se generó algún error al intentar copiar, se regresa a través de variables, para
                        // poder acceder a estos por medio de propiedades.
                        lngErrorReg = ArchivoError;
                        strErrorReg = ArchivoErrorDescripcion;
                        return false;
                    }
                }
                else
                {
                    // Si se generó algún error al intentar mapear, se regresa a través de variables, para
                    // poder acceder a estos por medio de propiedades.
                    lngErrorReg = DriveError;
                    strErrorReg = DriveErrorDescripcion;
                    return false;
                }
            }
            catch (Exception ex)
            {
                lngErrorReg = ex.HResult;
                strErrorReg = ex.Message;
                return false;
            }
            finally
            {
                // Desconecta la unidad si 'desconectarUnidad' es true.
                if (desconectarUnidad)
                {
                    DriveDesconectar(unidadLocal);
                }
            }
        }

        private static bool MapeoVerifica(string rutaRemota, string unidadLocal, string usuario = "", string contraseña = "")
        {
            try
            {
                NetworkCredential credenciales = new NetworkCredential(usuario, contraseña);

                NETRESOURCE nr = new NETRESOURCE
                {
                    dwType = 1, // RESOURCE_DISK
                    lpLocalName = unidadLocal,
                    lpRemoteName = rutaRemota
                };

                int result = WNetAddConnection2(ref nr, contraseña, usuario, 0);

                if (result == 0)
                {
                    DirectoryInfo di = new DirectoryInfo(unidadLocal);
                    if (di.Exists)
                    {
                        WNetCancelConnection2(unidadLocal, 0, false);
                        return true;
                    }
                    else
                    {
                        lngErrorReg = -1;
                        strErrorReg = "No se pudo acceder a la unidad local.";
                        WNetCancelConnection2(unidadLocal, 0, false);
                        return false;
                    }
                }
                else
                {
                    lngErrorReg = result;
                    strErrorReg = "Error al conectar la unidad de red.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                lngErrorReg = ex.HResult;
                strErrorReg = ex.Message;
                return false;
            }
        }

    }

}
