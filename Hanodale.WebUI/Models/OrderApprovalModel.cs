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

namespace Hanodale.WebUI.Models
{
    public class OrderApprovalModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public string orderNum { get; set; }
        public AccessRightsModel accessRight { get; set; }
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("ORDERAPPROVAL_ORDER_ID")]
        public string order_Id { get; set; }
        public TableProfileMetadataModel orderNum_Metadata { get; set; }

        
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("ORDERAPPROVAL_CUSTOMERNAME")]
        public string CustomerName { get; set; }
        public TableProfileMetadataModel CustomerName_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("ORDERAPPROVAL_REMARKS")]
        public string Remarks { get; set; }
        public TableProfileMetadataModel Remarks_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("ORDERAPPROVAL_ORDERDATE")]
        public Nullable<System.DateTime> OrderDate { get; set; }

        public TableProfileMetadataModel OrderDate_Metadata { get; set; }
       
        [CustomDisplayName("ORDERAPPROVAL_ORDERSTATUS")]
        public string OrderStatus { get; set; }
        public TableProfileMetadataModel OrderStatus_Metadata { get; set; }

  
        //[Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("ORDERAPPROVAL_ORDERTOTAL")]
        public decimal OrderTotal { get; set; }

        public TableProfileMetadataModel OrderTotal_Metadata { get; set; }

        [CustomDisplayName("ORDERAPPROVAL_SUBMITTEDUSER_ID")]
        public string CreatedBy { get; set; }

        public TableProfileMetadataModel CreatedBy_Metadata { get; set; }

        [CustomDisplayName("ORDERAPPROVAL_APPROVEDUSER_ID")]
        public string ApprovalBy { get; set; }

        [UIHint("ComboBox")]
        [CustomDisplayName("ORDERAPPROVAL_ORDERSTATUS")]
        public string ApprovalStatus { get; set; }
        public TableProfileMetadataModel ApprovalStatus_Metadata { get; set; }


        public TableProfileMetadataModel ApprovalBy_Metadata { get; set; }
        //public IEnumerable<SelectListItem> lstApprovalStatus { get; set; }
        public List<string> lstApprovalStatus { get; set; } // Add this property

        public int CustomerId { get; set; }

        public DateTime? ApprovalDate { get; set; }


        // Additional properties for OrderApproval model
        // You may add other properties as needed
    }
    public class OrderApprovalMaintenanceModel
    {
        public string record_Id { get; set; }
        public bool readOnly { get; set; }

        public TableProfileModel tableProfile { get; set; }

        public OrderApprovalModel orderApproval { get; set; }

        public ApprovalModel approval { get; set; }


    }

    public class ApprovalModel
    {
        public string remarks { get; set; }

    }
}