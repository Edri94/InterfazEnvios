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
    
    public partial class OPERACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OPERACION()
        {
            this.OPERACION_SWIFT = new HashSet<OPERACION_SWIFT>();
        }
    
        public int operacion1 { get; set; }
        public int producto_contratado { get; set; }
        public short operacion_definida { get; set; }
        public System.DateTime fecha_captura { get; set; }
        public byte status_operacion { get; set; }
        public System.DateTime fecha_operacion { get; set; }
        public decimal monto_operacion { get; set; }
        public short usuario_captura { get; set; }
        public Nullable<short> usuario_valida { get; set; }
        public Nullable<int> linea { get; set; }
        public Nullable<int> funcionario { get; set; }
        public string contacto { get; set; }
        public Nullable<int> grabadora { get; set; }
    
        public virtual COMPRA_CD COMPRA_CD { get; set; }
        public virtual COMPRA_TD_OVERNIGHT COMPRA_TD_OVERNIGHT { get; set; }
        public virtual DEPOSITO DEPOSITO { get; set; }
        public virtual DEPOSITO_CED DEPOSITO_CED { get; set; }
        public virtual DEPOSITO_PME DEPOSITO_PME { get; set; }
        public virtual OPERACION_DEFINIDA OPERACION_DEFINIDA1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPERACION_SWIFT> OPERACION_SWIFT { get; set; }
        public virtual OPERACION_PIU OPERACION_PIU { get; set; }
        public virtual PRODUCTO_CONTRATADO PRODUCTO_CONTRATADO1 { get; set; }
        public virtual REFERENCIAS REFERENCIAS { get; set; }
        public virtual REPORTE_SWIFT_MT103 REPORTE_SWIFT_MT103 { get; set; }
        public virtual RETIRO_CED RETIRO_CED { get; set; }
        public virtual RETIRO_PME RETIRO_PME { get; set; }
    }
}
