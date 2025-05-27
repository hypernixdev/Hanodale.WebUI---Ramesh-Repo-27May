using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs.Order
{
    public class OrderApiDto
    {
        public int? Id { get; set; }
        public string PosOrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string Plant { get; set; }
        public int CustNum { get; set; }
        public string districtID { get; set; }
        public string ShipToNum { get; set; }
        public string Remarks { get; set; }
        public string contact { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public bool OneTimeCustomer { get; set; }
        public List<OrderDetailApiDto> OrderDtl { get; set; }
        public List<PaymentDetail> paymentDtl { get; set; }
    }

    public class ScannedQtyModel
    {
        public decimal ScannedQty { get; set; }
        public string Barcode { get; set; }
    }

    public class OrderDetailApiDto
    {
        public int LineNo { get; set; }
        public string PartNum { get; set; }
        public decimal Quantity { get; set; }
        public string Uom { get; set; }
        public decimal? UnitPrice { get; set; }
        public bool IsLoose { get; set; }
        public bool IsFOC { get; set; }
        public bool IsSample { get; set; }
        public bool? IsExchange { get; set; }
        public bool? IsVaryWeight { get; set; }
        public bool IsStdFullQty { get; set; }
        public bool IsSlice { get; set; }
        public bool IsCube { get; set; }
        public bool IsStrip { get; set; }
        public decimal? SliceOrCubeCharges { get; set; }
        public string Remarks { get; set; }
        public decimal discountPercent { get; set; }

        public List<ScannedQtyModel> ScannedQtyList { get; set; }
    }

    public class PaymentDetail
    {
        public DateTime paymentDate { get; set; }
        public DateTime chequeDate { get; set; }
        public string paymentType { get; set; }
        public decimal totalAmt { get; set; }
        public decimal amount { get; set; }
        public string reference { get; set; }
        public string bank { get; set; }
        public string chequeNo { get; set; }
    }
}
