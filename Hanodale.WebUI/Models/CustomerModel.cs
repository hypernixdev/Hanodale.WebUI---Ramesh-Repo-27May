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
    public class CustomerModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public AccessRightsModel accessRight { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("CUSTOMER_NAME")]
        public string name { get; set; }
        public TableProfileMetadataModel name_Metadata { get; set; }


        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("CUSTOMER_ADDRESS1")]
        public string address1 { get; set; }
        public TableProfileMetadataModel address1_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("CUSTOMER_ADDRESS2")]
        public string address2 { get; set; }
        public TableProfileMetadataModel address2_Metadata { get; set; }


        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("CUSTOMER_CODE")]
        public string code { get; set; }
        public TableProfileMetadataModel code_Metadata { get; set; }

        [CustomDisplayName("CUSTOMER_CITY")]
        public string searchCity { get; set; }
        [CustomDisplayName("CUSTOMER_NAME")]
        public string searchName { get; set; }

        [CustomDisplayName("CUSTOMER_CODE")]
        public string searchCode { get; set; }

        [CustomDisplayName("CUSTOMER_STATE")]
        public string searchState { get; set; }

        [CustomDisplayName("CUSTOMER_COUNTRY")]
        public string searchCountry { get; set; }

        [UIHint("Number")]
        [CustomDisplayName("ORDERS_CODE")]
        public string searchOrderCode { get; set; }

        [UIHint("Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "--", DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [CustomDisplayName("ORDER_DATEFROM")]
        public Nullable<System.DateTime> searchOrderDateFrom { get; set; }

        [UIHint("Date")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "--", DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [CustomDisplayName("ORDER_DATETO")]
        public Nullable<System.DateTime> searchOrderDateTo { get; set; }

        [UIHint("ComboBox")]
        [CustomDisplayName("ORDERS_STATUS")]
        public string searchOrderStatus { get; set; }


        [CustomDisplayName("CUSTOMER_GROUPCODE")]

        public string groupCode { get; set; }
        public TableProfileMetadataModel groupCode_Metadata { get; set; }
        
       [CustomDisplayName("CUSTOMER_INDEX_CUSTID")]

        public string custID { get; set; }
        public TableProfileMetadataModel custID_Metadata { get; set; }

        // public IEnumerable<SelectListItem> lstEmployeeProfile { get; set; }       
    }
    public partial class CustomerMaintenanceModel
    {
        public string record_Id { get; set; }
        public bool readOnly { get; set; }

        public TableProfileModel tableProfile { get; set; }

        public CustomerModel Customer { get; set; }
    }
}