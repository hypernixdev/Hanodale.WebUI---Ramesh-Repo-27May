using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.SyncService.Models
{
    public class CustomerApiModel
    {
        [JsonProperty("company")]
        public string Company { get; set; }
        public string custID { get; set; }

        [JsonProperty("custNum")]
        public string code { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string groupCode { get; set; }
        public string tin { get; set; }

        public bool creditHold { get; set; }
    }
}
