using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.SyncService.Models
{
    public class PaymentReportApiModel
    {
        public DateTime paymentDate { get; set; } // To store the payment date
        public decimal epiCashEntryTotal { get; set; } // To store the total cash entry value
        public int epicorCountOfSalesOrders { get; set; } // To store the count of sales orders
        public decimal totalCash { get; set; } // To store the total cash amount
        public decimal totalCreditCard { get; set; } // To store the total credit card amount
        public decimal totalCheque { get; set; } // To store the total cheque amount
        public decimal totalDiscount { get; set; } // To store the total discount amount
        public decimal epiAdvanceEntryTotal { get; set; }
        public decimal totalAdvanceCash { get; set; } // To store the total advance cash amount
        public decimal totalAdvanceCreditCard { get; set; } // To store the total advance credit card amount
        public decimal totalAdvanceCheque { get; set; } // To store the total advance cheque amount
        public int totalEntries { get; set; } // To store the total number of entries
    }
}
