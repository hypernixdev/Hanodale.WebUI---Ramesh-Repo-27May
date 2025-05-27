using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs.Order
{
    public class CustomerPricing
    {
        public string code { get; set; }
        public int CustNum { get; set; }
        public string custID { get; set; }
        public decimal? CustGrpBasePrice { get; set; }
        public decimal? CustShipBasePrice { get; set; }
        public string UomCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ListCode { get; set; }
        public string PartNum { get; set; }
    }
}
