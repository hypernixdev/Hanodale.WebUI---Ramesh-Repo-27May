using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.SyncService.Models
{
    public class PriceListApiModel
    {
        [JsonProperty("listCode")]
        public string ListCode { get; set; }
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
        [JsonProperty("listDescription")]
        public string ListDescription { get; set; }
        [JsonProperty("startDate")]
        public string StartDate { get; set; }
        [JsonProperty("endDate")]
        public string EndDate { get; set; }
        public string sysRowID { get; set; }
    

   
    }
}
