﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CATALOGOSEntities : DbContext
    {
        public CATALOGOSEntities()
            : base("name=CATALOGOSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<PERFIL_HEXA> PERFIL_HEXA { get; set; }
        public virtual DbSet<PERMISOS_HEXA> PERMISOS_HEXA { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }
        public virtual DbSet<PERMISOS_X_USUARIO_HEXA> PERMISOS_X_USUARIO_HEXA { get; set; }
        public virtual DbSet<USUARIO_X_APLICACION> USUARIO_X_APLICACION { get; set; }
        public virtual DbSet<CENTRO_REGIONAL_KAPITI> CENTRO_REGIONAL_KAPITI { get; set; }
        public virtual DbSet<CODIGO_OPERACION_EQ> CODIGO_OPERACION_EQ { get; set; }
        public virtual DbSet<TIPO_CLIENTE> TIPO_CLIENTE { get; set; }
        public virtual DbSet<UBICACION> UBICACION { get; set; }
        public virtual DbSet<AGENCIA> AGENCIA { get; set; }
        public virtual DbSet<CLIENTE> CLIENTE { get; set; }
        public virtual DbSet<DIRECCION_ENVIO> DIRECCION_ENVIO { get; set; }
    }
}
