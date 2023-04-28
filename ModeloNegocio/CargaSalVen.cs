using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModeloNegocio
{
    public static class CargaSalVen
    {
        public static DateTime gsFechDiaSig;
        public static void CalcDiaSig()
        {
            int lnDiaSig;
            Datos.Parametros lsStatusInterfaz;
            string lsVerificaCargaSaldosHou;

            

            lnDiaSig = 1;


            lsStatusInterfaz = Parametro.GetParametrizacion("TRANSSTATUS");
            lsVerificaCargaSaldosHou = lsStatusInterfaz.error_saldos.ToString().Substring(1, 1);

            if(lsStatusInterfaz.valor != "OPEN" && lsVerificaCargaSaldosHou != "1")
            {
                gsFechDiaSig = ModeloNegocio.Funciones.SiguienteDiaHabil(Parametro.FechaServidor(), lnDiaSig);
                Log.Escribe("Dia siguiente habil: " +  gsFechDiaSig);
            }
            else
            {
                gsFechDiaSig = lsStatusInterfaz.fecha_sistema;
            }

        }
    }
}
