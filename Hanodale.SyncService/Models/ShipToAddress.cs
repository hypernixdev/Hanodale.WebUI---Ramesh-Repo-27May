using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.SyncService.Models
{
    public class ShipToAddressApiModel
    {

        [JsonProperty("company")]
        public string plantName { get; set; }
        [JsonProperty("custNum")]
        public string custID { get; set; }
        [JsonProperty("shipToNum")]
        public string shippingCode { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }

        /*company": "LUCKY00",
"custNum": 22690,
"shipToNum": "BARLAI/BB",
"name": "",
"address1": "NO.3 JALAN SIN CHEW KEE",
"address2": "BUKIT BINTANG",
"address3": "",
"city": "50150 KUALA LUMPUR (TEL:2141 7850)",
"state": "",
"zip": "",
"country": "MALAYSIA"
*/
    }
}
