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
    
    public partial class COMPRA_TD_OVERNIGHT
    {
        public int operacion { get; set; }
        public System.DateTime fecha_vencimiento { get; set; }
        public decimal tasa { get; set; }
        public byte origen { get; set; }
        public string referencia_detalle { get; set; }
        public Nullable<short> plazo { get; set; }
        public Nullable<int> operacion_a_renovar { get; set; }
        public Nullable<byte> tipo_tasa { get; set; }
        public Nullable<int> operacion_venc { get; set; }
    
        public virtual OPERACION OPERACION1 { get; set; }
    }
}
