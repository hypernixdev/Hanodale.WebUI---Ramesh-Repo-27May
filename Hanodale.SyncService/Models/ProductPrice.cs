using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.SyncService.Models
{
    public class ProductPriceApiModel
    {
        public string PartNum { get; set; }
        public decimal BasePrice { get; set; }
        public string Company { get; set; }
        public string uomCode { get; set; }
        public string ListCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CurrencyCode { get; set; }

        public bool is404 { get; set; } = false;
        public bool isNetWorkError { get; set; } = false;
    }
}
