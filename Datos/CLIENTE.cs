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
    
    public partial class CLIENTE
    {
        public string cuenta_cliente { get; set; }
        public string tratamiento { get; set; }
        public string nombre_cliente { get; set; }
        public string direccion_cliente { get; set; }
        public string cp_cliente { get; set; }
        public string telefono_cliente { get; set; }
        public string fax_cliente { get; set; }
        public string tipo_cliente { get; set; }
        public System.DateTime fecha_alta { get; set; }
        public Nullable<System.DateTime> fecha_baja { get; set; }
        public Nullable<short> ubicacion { get; set; }
        public int funcionario { get; set; }
        public Nullable<byte> tipo_retencion { get; set; }
        public string rfc { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public byte persona_moral { get; set; }
        public string UNIDAD_TEMP { get; set; }
        public string colonia_cliente { get; set; }
        public Nullable<byte> cuenta_modificada { get; set; }
        public Nullable<byte> cuenta_mancomunada { get; set; }
        public string cuenta_eje_pesos { get; set; }
        public string mnemonico { get; set; }
        public Nullable<byte> deposito_inicial { get; set; }
        public string descripcion_deposito_inicial { get; set; }
        public string cuenta_mercury { get; set; }
        public Nullable<decimal> monto_deposito_inicial { get; set; }
        public Nullable<short> documentacion { get; set; }
        public Nullable<byte> agencia { get; set; }
        public Nullable<byte> tiene_chequera { get; set; }
        public string cuenta_houston { get; set; }
        public Nullable<System.DateTime> fecha_banklink { get; set; }
        public Nullable<System.DateTime> fecha_cuenta_pesos { get; set; }
        public Nullable<int> funcionario_pesos { get; set; }
        public Nullable<decimal> deposito_inicial_fed { get; set; }
        public Nullable<decimal> deposito_inicial_suc { get; set; }
        public string func_pesos { get; set; }
        public string calle { get; set; }
        public string no_ext { get; set; }
        public string no_int { get; set; }
        public string componente { get; set; }
        public string del_o_municipio { get; set; }
        public string shortname { get; set; }
        public string curp { get; set; }
    
        public virtual TIPO_CLIENTE TIPO_CLIENTE1 { get; set; }
    }
}
