using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaBloomberg.BE
{
    public class PreciosACargarBE
    {
        public DateTime FechaCarga { get; set; }
        public List<PrecioMercTmpBE> LstPrecios { get; set; }

        public PreciosACargarBE()
        {
            DateTime scrap = DateTime.Now;
            this.FechaCarga = new DateTime(scrap.Year, scrap.Month, scrap.Day);
            this.LstPrecios = new List<PrecioMercTmpBE>();
        }

        public PreciosACargarBE(DateTime pfecCarga)
        {
            this.FechaCarga = pfecCarga;
            this.LstPrecios = new List<PrecioMercTmpBE>();
        }
    }
}
