using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.SyncService.Models
{
    public class PriceListPartApiModel
    {
        public string ListCode { get; set; }
        public string PartNum { get; set; }
        public decimal BasePrice { get; set; }
        public string UomCode { get; set; }
        public string sysRowID { get; set; }
    }

}
