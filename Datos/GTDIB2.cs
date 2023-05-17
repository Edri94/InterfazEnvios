using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class GTDIB2
    {
        public int operacion { get; set; }
        public string ref_tipo_cd { get; set; }
        public int? sucursal_kapiti { get; set; }
        public string cuenta_cliente { get; set; }
        public string sufijo_kapiti { get; set; }
        public decimal? monto_operacion { get; set; }
        public DateTime? fecha_operacion { get; set; }
        public DateTime? fecha_vencimiento { get; set; }
        public decimal? tasa { get; set; }
        public string nombre_cliente { get; set; }
        public int? usuario_captura { get; set; }
        public string cr { get; set; }
        public string kapiti_ctp { get; set; }
        public int? kapiti_c2r { get; set; }
        public string cuenta { get; set; }
        public int? funcionario { get; set; }
        public int unidad_org { get; set; }
        public int unidad_org_padre { get; set; }
        public int tipo_unidad_org { get; set; }
        public string centro_reg_kap { get; set; }
        public string origen { get; set; }
        public string unidad_org_bancomer { get; set; }
        public int operacionAPX { get; set; }
    }
}
