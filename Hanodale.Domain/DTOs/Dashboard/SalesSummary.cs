using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class SalesSummaryResult
    {
        public List<PendingSalesTable> PendingSalesTable { get; set; }
        public List<CompletedSalesTable> CompletedSalesTable { get; set; }
        public List<SyncCompletedSalesTable> SyncCompletedSalesTable { get; set; }
        public List<PaymentCollectionTable> PaymentCollectionTable { get; set; }

    }

    public class PendingSalesTable
    {
        public int? OrdersCount { get; set; }
        public decimal? SalesAmount { get; set; }

    }
    public class CompletedSalesTable
    {
        public int? OrdersCount { get; set; }
        public decimal? SalesAmount { get; set; }
        public decimal? CollectedAmount { get; set; }
        public decimal? DiscRounding { get; set; }
        public decimal? RefundToCustomer { get; set; }
        public decimal? RefundAsAdvance { get; set; }
        public decimal? Diff { get; set; }

    }
    public class SyncCompletedSalesTable
    {
        public int? OrdersCount { get; set; }
        public decimal? SalesAmount { get; set; }
        public decimal? SalesPaymentAmount { get; set; }
        public int? EpiCount { get; set; }
        public decimal? EpicorSalesAmount { get; set; }
        public decimal? EpicorAppliedAmount { get; set; }
        public decimal? EpicorAdvanceAmount { get; set; }
        public decimal? DiffSalesVsEpicor { get; set; }
        public decimal? DiffPaymentVsEpicor { get; set; }
        public string SyncStatus { get; set; }

    }
    public class PaymentCollectionTable
    {
        public string PaymentType { get; set; }
        public decimal? CollectedAmount { get; set; }
        public decimal? RefundToCustomer { get; set; }
        public decimal? RefundAsAdvance { get; set; }


    }
}
