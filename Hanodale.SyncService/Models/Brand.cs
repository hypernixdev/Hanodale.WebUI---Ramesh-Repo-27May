using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.SyncService.Models
{
    public class BrandApiModel
    {
        public string company { get; set; }
        public string brandCode { get; set; }
        public string brandDescription { get; set; }
        public string sysRowID { get; set; }

    }
}
