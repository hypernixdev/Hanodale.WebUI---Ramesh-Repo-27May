using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.SyncService.Models
{
    public class CustomerPriceListApiModel
    {
        public int CustNum { get; set; }
        public string ShipToNum { get; set; }
        public int SeqNum { get; set; }
        public string ListCode { get; set; }
        public string sysRowID { get; set; }
    }

}
