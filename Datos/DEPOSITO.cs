//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Datos
{
    using System;
    using System.Collections.Generic;
    
    public partial class DEPOSITO
    {
        public int operacion { get; set; }
        public byte en_firme { get; set; }
        public byte destino { get; set; }
        public string otros { get; set; }
        public Nullable<byte> tipo_documento { get; set; }
        public string otro_documento { get; set; }
        public Nullable<byte> tipo_moneda { get; set; }
        public Nullable<int> folio_linea_servicio { get; set; }
        public Nullable<int> referencia_ced { get; set; }
        public string causa { get; set; }
    
        public virtual OPERACION OPERACION1 { get; set; }
        public virtual TIPO_DOCUMENTO TIPO_DOCUMENTO1 { get; set; }
    }
}