using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Orders
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string orderNum { get; set; }
        [DataMember]
        public string customerName { get; set; }
        
        [DataMember]
        public string shipToAddressCode { get; set; }
        [DataMember]
        public Nullable<DateTime> orderDate { get; set; }
        [DataMember]
        public bool isSuccess { get; set; }
        [DataMember]
        public Nullable<decimal> orderTotal { get; set; }
        [DataMember]
        public string orderStatus { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public Nullable<DateTime> createdDate { get; set; }
        [DataMember]
        public Nullable<DateTime> pickerDate { get; set; }

        [DataMember]
        public Nullable<DateTime> paymentDate { get; set; }
        [DataMember]
        public string picker { get; set; }
        [DataMember]
        public string payment { get; set; }

        public int pickerUserId { get; set; }

        [DataMember]
        public int customer_Id { get; set; }
        [DataMember]
        public string orderComment { get; set; }

        [DataMember]
        public bool? syncStatus { get; set; }

        [DataMember]
        public Nullable<DateTime> syncedAt { get; set; }


        [DataMember]
        public string epicoreResponse { get; set; }

        [DataMember]
        public int? postStatus { get; set; }      

    }
   
    public class OrderDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<Orders> lstOrder { get; set; }
    }

    public class SubmitOrder
    {
        public string id { get; set; }
        public int orderNum { get; set; }
   
        public int customer_Id { get; set; }
        public int shipToAddress_Id { get; set; }
        public int districtId { get; set; }

        public DateTime orderDate { get; set; }

        public string orderComment { get; set; }
        public string orderStatus { get; set; }
        public string entryPerson { get; set; }
        public bool oneTimeCustomer { get; set; }
        public string orderContact { get; set; }
        public string orderContactName { get; set; }
        public string orderContactPhone { get; set; }
        public List<SubmitOrderItem> OrderItems { get; set; }
    }

    public class UpdateOrder : SubmitOrder
    {
        public List<int> RemovedOrderItemIds { get; set; }
    }

    public class SubmitOrderItem
    {
        public int product_Id { get; set; }
        public int orderId { get; set; }

        public string partNum { get; set; }
        public string lineDesc { get; set; }
        public string ium { get; set; }
        public string salesUm { get; set; }
        public decimal unitPrice { get; set; }
        public decimal orderQty { get; set; }
        public decimal? scannedQty { get; set; }
        public decimal? discountPer { get; set; }
        public decimal? discountAmt { get; set; }
        public decimal discount { get; set; }
        public decimal listPrice { get; set; }
        public decimal returnTotal { get; set; }


        public int QtyType_ModuleItem_Id { get; set; }


        public int OrderUOM_Id { get; set; }
        public int operationStyle_ModuleItem_Id { get; set; }
        public decimal operationCost { get; set; }
        public decimal actualOperationCost { get; set; }
        public int complimentary_ModuleItem_Id { get; set; }
        public bool allowVaryWeight { get; set; }
        public decimal originalUnitPrice { get; set; }
        public decimal realOriginalUnitPrice { get; set; }
        public int orderItemId { get; set; }

        public string comments { get; set; }
        public string scannedLabel { get; set; }
        public string scannedLocation { get; set; }
        public int createdBy { get; set; }
        public DateTime createdAt { get; set; }
    }


    public class ViewOrder
    {
        public string id { get; set; }

        public string orderId { get; set; }

        public int customer_Id { get; set; }

        public string customerName { get; set; }
        public string orderNum { get; set; }
        public int? shipToAddress_Id { get; set; }
        public int? districtId { get; set; }
        public string shipToName { get; set;  }
        public DateTime? orderDate { get; set; }
        public string docOrderAmt { get; set; }
        public string entryPerson { get; set; }
        public string orderComment { get; set; }
        public string orderStatus { get; set; }
        public string verifiedStatus { get; set; }
        public string verifiedBy { get; set; }
        public string verifyRemarks { get; set; }
        public DateTime? verifiedDate { get; set; }
        public List<ViewOrderItem> OrderItems { get; set; }
        public List<ViewOrderItemDeleted> OrderItemDeleted { get; set; }
        public List<OrderPayments> OrderPayments { get; set; }
        public List<OrderScanned> OrderScanned { get; set; }

        public string cancelRemarks { get; set; }

        public bool oneTimeCustomer { get; set; }
        public string orderContact { get; set; }
        public string orderContactName { get; set; }
        public string orderContactPhone { get; set; }

        public string tinId { get; set; }
        public string customerAddress { get; set; }
        public string shipToAddress { get; set; }
        public string epicoreResponse { get; set; }
        public string epicoreOrderId { get; set; }
        public string shipPackNumber { get; set; }
        public DateTime? syncedAt { get; set; }
        public string epicoreInvNumber { get; set; }
        public string UD16Key1 { get; set; }
        public bool syncStatus { get; set; }
        public OrderApprovals orderApprovals { get; set; }

        public bool creditHold { get; set; }

    }

    public class ViewOrderItem
    {
        public int orderLine { get; set; }
        public int product_Id { get; set; }
        public int itemId { get; set; }
        public string partNum { get; set; }

        public string partName { get; set; }
        public string prodGroup { get; set; }
        public string lineDesc { get; set; }
        public string ium { get; set; }
        public string salesUm { get; set; }

        public string orderUOM { get; set; }
        public string orderType { get; set; }
        public string complementary { get; set; }
        public string operationName { get; set; }

        public decimal scannedQty { get; set; }
        public decimal discountPer { get; set; }
        public decimal discountAmt { get; set; }

        public string AllowSellingVaryWeight     { get; set; }
        public decimal unitPrice { get; set; }
        public decimal orderQty { get; set; }
        public decimal discount { get; set; }
        public decimal listPrice { get; set; }
        public decimal returnTotal { get; set; }

        public int? QtyType_ModuleItem_Id { get; set; }
        public string comments { get; set; }

        public int? OrderUOM_Id { get; set; }
        public int? operationStyle_ModuleItem_Id { get; set; }
        public decimal operationCost { get; set; }
        public decimal actualOperationCost { get; set; }
        public int? complimentary_ModuleItem_Id { get; set; }
        public int  orderId { get; set; }
        public virtual List<OrderScanned> OrderItemScanned { get; set; }
        public bool? allowVaryWeight { get; set;  }
        public decimal originalUnitPrice { get; set;  }
        public decimal realOriginalUnitPrice { get; set; }
        public int orderItemId { get; set; }
        public bool IsReturned { get; set; }

        public string country { get; set; }

        public string scannedQtyStr { get; set; }
        public Nullable<decimal> avlQty { get; set; }
        public string scannedLabel { get; set; }
        public string scannedLocation { get; set; }
        public string itemBrandCode { get; set; }
        public string itemBrandName { get; set; }
        public decimal conversionFactor { get; set; }

        public DateTime? createdAt { get; set; }
        public string createdBy { get; set; }
        public int? createdById { get; set; }
    }

    public class ViewOrderItemDeleted: ViewOrderItem
    {
        public DateTime? deletedAt { get; set; }
        public string deletedBy { get; set; }
    }

    public class OrderScanned
    {
        public int Id { get; set; }
        public int orderItem_Id { get; set; }
        public int orderId { get; set; }

        public string serialNo { get; set; }
        public decimal scannedQty { get; set; }
        public string status { get; set; }
        public string verifyStatus { get; set; }
        public string scannedBy { get; set; }
        public DateTime scannedDate { get; set; }

        public int userId { get; set; }

        public string partNo { get; set; }

        public string partName { get; set; }
        public string Group { get; set; }
        public string orderUOM { get; set; }
        public bool IsReturned { get; set; }

        public bool allowVaryWeight { get; set; }
        public decimal returnQty { get; set; }

        public string productLocation { get; set; }
        public List<string> LocationOptions { get; set; }

    }

    public class OrderPayments
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string PaymentType { get; set; }
        public decimal Amount { get; set; }
        public string RefNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string Bank { get; set; }
        public int UserId { get; set; }

        public string Payment { get; set; }

        public bool IsRefund { get; set; }
    }

    public class CreateOrderPayment
    {
        public int OrderId { get; set; }
        public string PaymentType { get; set; }
        public decimal Amount { get; set; }
        public string RefNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string Bank { get; set; }
        public int UserId { get; set; }

        public string Payment { get; set; }

        public bool IsRefund { get; set; }

    }
    public class OrderUOM
    {
        public int id { get; set; }
        public string code { get; set; }
        public bool? disableLooseQty { get; set; }
        public bool? disableFullQty { get; set; }

     
    }

    public class ProductBarcode
    {
        public int id { get; set; }

        public int orderItemId { get; set; }
        public string epicorePartNo { get; set; }
        public string barcode { get; set; }
        public string weightValue { get; set; }
        public decimal? weightMutiplier { get; set; }
        public int productId { get; set; }
        public int weightFromPos { get; set; }
        public int weightToPos { get; set; }
        public int barcodeFromPos { get; set; }
        public int barcodeToPos { get; set; }

        public bool? allowVaryWeight { get; set; }

        // New for Location binding
        public List<string> LocationList { get; set; }
        public string Location { get; set; }

        public string productLocation { get; set; }
        public decimal CtnQty { get; set; }
        public decimal PickedCtnQty { get; set; }
        public bool IsPickedComplete { get; set; }
        public bool IsCarton { get; set; }
        public bool IsPartialCarton { get; set; }
        public decimal IumQty { get; set; }
        public string Ium { get; set; }
        public decimal LooseQty { get; set; }
        public string LooseUom { get; set; }
        public bool IsVaryWg { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public long SeqNum { get; set; }
        public bool OnHold { get; set; }
    }

    #region Scan order dto
    public class SubmitReturnItems
    {
        public int OrderId { get; set; }
        public decimal orderTotal { get; set; }
        public List<ScannedItem> ReturnedItems { get; set; } = new List<ScannedItem>();
        public List<ReturnOrderItems> ReturnOrderItems { get; set; } = new List<ReturnOrderItems>();
    }

    public class ReturnOrderItems
    {
        public int OrderItemId { get; set; }
        public decimal cuttingCost { get; set; }
        public decimal discountAmt { get; set; }
        public decimal listPrice { get; set; }
        public decimal returnTotal { get; set; }
    }

    public class SubmitOrderScan
    {
        public int OrderId { get; set; }
        public List<ScannedItem> ScannedItems { get; set; }
        public List<ScanOrderItem> OrderItems { get; set; }
        public string action { get; set; }
    }

    public class ScannedItem
    {
        public int OrderId { get; set; }
        public int OrderItemId { get; set; }
        public int scannedId { get; set; }
        public string PartNum { get; set; }
        public string SerialNumber { get; set; }
        public string OrderUOM { get; set; }
        public string Status { get; set; }
        public string ProdGroup { get; set; }
        public decimal ScannedQty { get; set; }
        public decimal returnQty { get; set; }
        public string OrderType { get; set; }
        public string productLocation { get; set; }  // ✅ Add this line

    }

    public class ScanOrderItem
    {
        public int OrderItemId { get; set; }
        public int itemId { get; set; }
        public decimal ScannedQty { get; set; }
    }
    #endregion

    public class OrderSyncStatusDto
    {
        public int OrderId { get; set; }
        public string EpicoreOrderId { get; set; }
        public string ShipPackNumber { get; set; }
        public bool SyncStatus { get; set; }
        public string SyncMessage { get; set; }
        public string UD16Key1 { get; set; }  // Added UD16Key1 field
        public string EpicoreInvNumber { get; set; }  // Added Epicore Invoice Number field
    }
}
