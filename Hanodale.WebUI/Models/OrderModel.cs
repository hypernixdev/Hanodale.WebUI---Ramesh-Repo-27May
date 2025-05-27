using System;
using System.Collections.Generic;
using Hanodale.Domain.DTOs;
using System.ComponentModel.DataAnnotations;
using Hanodale.Utility.Globalize;
using System.Web.Mvc;
using Hanodale.WebUI.Helpers;
using System.ComponentModel;
using Hanodale.BusinessLogic;
using Microsoft.Practices.ServiceLocation;
using Hanodale.Entity.Core;

namespace Hanodale.WebUI.Models
{
    public class OrderModel
    {
        public string id { get; set; }
        public int customer_Id { get; set; }
        public bool isEdit { get; set; }
        public AccessRightsModel accessRight { get; set; }
        public bool readOnly { get; set; }   
      
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("ORDER_CODE")]
        [StringLength(100, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string orderCode { get; set; }

        public TableProfileMetadataModel orderCode_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("ORDER_PRICETIER")]
        [StringLength(100, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string priceTier { get; set; }

        public TableProfileMetadataModel priceTier_Metadata { get; set; }

       
        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("ORDER_SHIPTOADDRESS")]
        public Nullable<int> shipToAddress_Id { get; set; }
        public TableProfileMetadataModel shipToAddress_Id_Metadata { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [CustomDisplayName("ORDER_DATE")]
        public Nullable<System.DateTime> orderDate { get; set; }


        public TableProfileMetadataModel orderDate_Metadata { get; set; }

        public IEnumerable<SelectListItem> lstshipToAddress { get; set; }
        public static List<Orders> lstOrder { get; set; }

    }

    #region submit order model 
    public class SubmitOrderModel
    {
        public string id { get; set; }
        public int customer_Id { get; set; }
        public int shipToAddress_Id { get; set; }
        public int districtId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? orderDate { get; set; }
        public string orderComment { get; set; }
        public string orderStatus { get; set; }
        public List<SubmitOrderItemModel> OrderItems { get; set; }

        public bool oneTimeCustomer { get; set; }
        public string orderContact { get; set; }
        public string orderContactName { get; set; }
        public string orderContactPhone { get; set; }
    }

    public class UpdateOrderModel: SubmitOrderModel
    {
        public List<int> RemovedOrderItemIds { get; set; }
    }

    public class SubmitOrderItemModel
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

        public decimal originalUnitPrice { get; set;  }

        public decimal realOriginalUnitPrice { get; set; }

        public int orderItemId { get; set; }

        public string comments { get; set; }
        public string scannedLabel { get; set; }
        public string scannedLocation { get; set; }
        public int createdBy { get; set; }
        public DateTime createdAt { get; set; }
    }

    public class ViewOrderModel
    {
        public bool isEdit { get; set; }
        public AccessRightsModel accessRight { get; set; }
        public bool readOnly { get; set; }
        public string id { get; set; }
        public int customer_Id { get; set; }
        public string customerName { get; set; }
        public string orderNum { get; set; }
        public int? shipToAddress_Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string orderDate { get; set; }
        public string orderComment { get; set; }
        public string docOrderAmt { get; set; }
        public string entryPerson { get; set; }
        public string orderStatus { get; set; }

        public string verifiedStatus { get; set; }
        public string verifiedBy { get; set; }
        public string verifyRemarks { get; set; }
        public string verifiedDate { get; set; }
        public List<ViewOrderItemModel> OrderItems { get; set; }
        public List<OrderPaymentModel> OrderPayments { get; set; }
        public List<OrderScannedModel> OrderScanned { get; set; }

        public decimal minTolerance { get; set; }
        public decimal maxTolerance { get; set; }
        public bool DisableScannedQtyValidation { get; set; }

        public string tinId { get; set; }
        public string customerAddress { get; set; }
        public string shipToAddress { get; set; }
        public string shipToName { get; set; }
    }

    public class ViewOrderItemModel
    {
        public int orderLine { get; set; }
        public int product_Id { get; set; }
        public int itemId { get; set; }
        public int itemDbId { get; set; }
        public int orderId { get; set; }
        public string partNum { get; set; }
        public string prodGroup { get; set; }
        public string description { get; set; }

        public string code { get; set; }
        public string lineDesc { get; set; }
        public string ium { get; set; }
        public string comments { get; set; }

        public string salesUm { get; set; }
        public decimal unitPrice { get; set; }
        public decimal orderQty { get; set; }
        public decimal discount { get; set; }
        public decimal listPrice { get; set; }
        public decimal returnTotal { get; set; }
        public string AllowSellingVaryWeight { get; set; }
        public int? QtyType_ModuleItem_Id { get; set; }
        public string orderUOM { get; set; }
        public string orderType { get; set; }
        public string complementary { get; set; }
        public string operationName { get; set; }

        public decimal scannedQty { get; set; }
        public decimal discountPer { get; set; }
        public decimal discountAmt { get; set; }

        public int? OrderUOM_Id { get; set; }
        public int? operationStyle_ModuleItem_Id { get; set; }
        public decimal operationCost { get; set; }
        public decimal actualOperationCost { get; set; }
        public int? complimentary_ModuleItem_Id { get; set; }
        public string allowVaryWeight { get; set; }
        public decimal originalUnitPrice { get; set; }
        public decimal realOriginalUnitPrice { get; set; }

        public int orderItemId { get; set; }

        public bool IsReturned { get; set; }
        public string scanQtyStr { get; set; }

        public Nullable<decimal> avlQty { get; set; }

        public string scannedLabel { get; set; }
        public string scannedLocation { get; set; }

        public string itemBrandCode { get; set; }
        public string itemBrandName { get; set; }

        public string country { get; set; }

        public decimal retQty { get; set; }

        public decimal retTotal { get; set; }
        public decimal conversionFactor { get; set; }
        public DateTime? createdAt { get; set; }
        public string createdBy { get; set; }
        public List<string> LocationList { get; set; }
        public string productLocation { get; set; }

        public int? createdById { get; set; }
    }

    public class ViewOrderItemDeletedModel : ViewOrderItemModel
    {
        public DateTime? deletedAt { get; set; }
        public string deletedBy { get; set; }
    }

    public class OrderPaymentModel
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

        public bool IsRefund { get; set; }
    }
    public class OrderScannedModel
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
        public string partNo { get; set; }

        public string partName { get; set; }
        public string Group { get; set; }
        public string orderUOM { get; set; }

        public bool IsReturned { get; set; }
        public bool allowVaryWeight { get; set; }
        public decimal returnQty { get; set; }
        public List<SelectListItem> LocationOptions { get; set; } = new List<SelectListItem>();
        public string productLocation { get; set; } // selected value

    }


    public class CreateOrderPaymentModel
    {
        public string PaymentType { get; set; }
        public decimal Amount { get; set; }
        public string RefNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string Bank { get; set; }
    }

    public class UpdateOrderPayments
    {
        public List<OrderPayments> OrderPayments { get; set; } = new List<OrderPayments>();
        public bool IsOrderComplete { get; set; }
        public bool IsRefund {  get; set; }
    }
    #endregion
    #region Edit order model 

    public class EditOrderModel
    {
        public bool isEdit { get; set; }
        public bool isClone { get; set; }
        public bool IsSuperAdminLogin { get; set; }
        public AccessRightsModel accessRight { get; set; }
        public bool readOnly { get; set; }
        public string id { get; set; }
        public string encryptedId { get; set; }
        public int customer_Id { get; set; }
        public string customerName { get; set; }
        public string orderNum { get; set; }
        public int? shipToAddress_Id { get; set; }
        public int? districtId { get; set; }
        public string shipToName { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string orderDate { get; set; }
        public string orderComment { get; set; }
        public string docOrderAmt { get; set; }
        public string entryPerson { get; set; }
        public string orderStatus { get; set; }

        public string verifiedStatus { get; set; }
        public string verifiedBy { get; set; }
        public string verifyRemarks { get; set; }
        public string verifiedDate { get; set; }
        public List<ViewOrderItemModel> OrderItems { get; set; }
        public List<ViewOrderItemDeletedModel> OrderItemDeleted { get; set; }
        public List<OrderPaymentModel> OrderPayments { get; set; } = new List<OrderPaymentModel>();
        public List<OrderScannedModel> OrderScanned { get; set; }
        public bool IsCashierLogin { get; set; }

        public bool oneTimeCustomer { get; set; }
        public string orderContact { get; set; }
        public string orderContactName { get; set; }
        public string orderContactPhone { get; set; }
        public int? defaultCustomerId { get; set; }
        public int? defaultShipTo { get; set; }

        public bool ValidateStockBalance { get; set; }

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
        public bool disableComplimentary { get; set; }
        public bool creditHold { get; set; }


        public List<ShipToAddresses> shipToAddresses { get; set; }

        public string ApproverName { get; set; }

        public DateTime? ApproverDate { get; set; }
        public string ApprovalRemarks { get; set; }

        public string createdBy { get; set; }

        public DateTime orderDateFrom { get; set; }

        public DateTime orderDateTo { get; set; }

        public IEnumerable<SelectListItem> lstPaymentTypes { get; set; }
        public IEnumerable<SelectListItem> lstPaymentReturnTypes { get; set; }
    }
    #endregion

    #region Order Scanning model 

    public class SubmitReturnedModel
    {
        public int OrderId { get; set; }
        public decimal orderTotal { get; set; }
        public bool IsVerification { get; set; }
        public List<ScannedItemModel> ReturnedItems { get; set; } = new List<ScannedItemModel>();
        public List<ReturnOrderItemsModel> ReturnOrderItems { get; set; } = new List<ReturnOrderItemsModel>();
    }

    public class ReturnOrderItemsModel
    {
        public int OrderItemId { get; set; }
        public decimal cuttingCost { get; set; }
        public decimal discountAmt { get; set; }
        public decimal listPrice { get; set; }
        public decimal returnTotal { get; set; }
    }

    public class SubmitOrderScanModel
    {
        public int OrderId { get; set; }

        public bool IsVerification { get; set; }
        public string action { get; set; }
        public List<ScannedItemModel> ScannedItems { get; set; } = new List<ScannedItemModel> { };
        public List<ScanOrderItemModel> OrderItems { get; set; } = new List<ScanOrderItemModel> { };
    }

    public class ScannedItemModel
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
        public string Location { get; set; }
    }

    public class ScanOrderItemModel
    {
        public int OrderItemId { get; set; }
        public int itemId { get; set; }
        public decimal ScannedQty { get; set; }
    }
    #endregion
}