using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.SyncService.Models
{
    
    //public class OrderModel
    //{
    //    public int PosOrderNumber { get; set; }
    //    public DateTime OrderDate { get; set; }
    //    public string Plant { get; set; }
    //    public int CustNum { get; set; }
    //    public string ShipToNum { get; set; }
    //    public string Remarks { get; set; }
    //    public List<OrderDetailModel> OrderDtl { get; set; }

    //}

    //public class OrderDetailModel
    //{
    //    public int LineNo { get; set; }
    //    public string PartNum { get; set; }
    //    public int Quantity { get; set; }
    //    public string Uom { get; set; }
    //    public decimal UnitPrice { get; set; }
    //    public bool IsLoose { get; set; }
    //    public bool IsFOC { get; set; }
    //    public bool IsSample { get; set; }
    //    public bool IsVaryWeight { get; set; }
    //    public bool IsStdFullQty { get; set; }
    //    public bool IsSlice { get; set; }
    //    public bool IsCube { get; set; }
    //    public int SliceOrCubeCharges { get; set; }
    //    public string Remarks { get; set; }

    //}

    public class OrderResult
    {
        public int OrderNumber { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class ShipmentResult
    {
        public int PackNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class PaymentResult
    {
        public List<string> Key1 { get; set; }  // Added missing PaymentResult class with Key1
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string Key1AsCommaSeparated => string.Join(", ", Key1);
    }

    public class ApiResponse
    {
        public OrderResult OrderResult { get; set; }
        public ShipmentResult ShipmentResult { get; set; }
        public PaymentResult PaymentResult { get; set; }
    }

    public class ApiResponseMessage
    {
        public ApiResponse Message { get; set; }
    }

}
