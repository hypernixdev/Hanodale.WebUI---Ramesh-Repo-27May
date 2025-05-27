using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.SyncService.Models
{
    public class StockBalanceApiModel
    {
        public string company { get; set; }
        public string partNum { get; set; }
        public string warehouseCode { get; set; }
        public string uom { get; set; }
        public Decimal onHandQty { get; set; }

        [JsonIgnore] 
        public string uniqueField
        {
            get
            {
                return $"{company}-{partNum}-{uom}";
            }
        }
    }
}
