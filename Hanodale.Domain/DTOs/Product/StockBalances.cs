using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class StockBalances
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string company { get; set; }

        [DataMember]
        public string partNum { get; set; }

        [DataMember]
        public string warehouseCode { get; set; }

        [DataMember]
        public string uom { get; set; }

        [DataMember]
        public string uniqueField { get; set; }

        [DataMember]
        public string location { get; set; }


        [DataMember]
        public decimal onHandQty { get; set; }

        [DataMember]
        public Nullable<decimal> totalQtyBeforePayment { get; set; }

        [DataMember]
        public Nullable<decimal> totalQtyAfterPayment { get; set; }

        [DataMember]
        public bool isSuccess { get; set; }
    }

    public class StockBalanceDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<StockBalances> lstStockBalance { get; set; }
    }
}
