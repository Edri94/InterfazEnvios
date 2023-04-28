using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ParametrosEjecucion
    {
        public DateTime Fecha_Servidor { get; set; }
        public DateTime Fecha_Sistema { get; set; }
        public Int16 MT103 { get; set; }
        public Int16 CTIB { get; set; }
        public Int16 DRT { get; set; }
        public Int16 TDs { get; set; }
        public Int16 HOLD { get; set; }
        public Int16 Swift { get; set; }
        public Int16 MT202 { get; set; }
    }
}
