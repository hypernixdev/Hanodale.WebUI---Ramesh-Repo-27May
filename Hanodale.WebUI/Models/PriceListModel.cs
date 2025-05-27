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
    public class PriceListModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public AccessRightsModel accessRight { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRICELIST_LIST_CODE")]
        public string listCode { get; set; }
        public TableProfileMetadataModel listCode_Metadata { get; set; }


        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRICELIST_CURRENCY_CODE")]
        public string currencyCode { get; set; }
        public TableProfileMetadataModel currencyCode_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRICELIST_LISTDESCRIPTION")]
        public string listDescription { get; set; }
        public TableProfileMetadataModel listDescription_Metadata { get; set; }

        [UIHint("Date")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRICELIST_STARTDATE")]
        public Nullable<System.DateTime> startDate { get; set; }
        public TableProfileMetadataModel startDate_Metadata { get; set; }

        [UIHint("Date")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRICELIST_ENDDATE")]
        public Nullable<System.DateTime> endDate { get; set; }
        public TableProfileMetadataModel endDate_Metadata { get; set; }

        [CustomDisplayName("PRICELIST_CUSTOMERCODE")]
        public int? custNum { get; set; } //custId
        public TableProfileMetadataModel custNum_Metadata { get; set; }

        [CustomDisplayName("PRICELIST_SHIPPINGCODE")] 
        public string shipToNum { get; set; }
        public TableProfileMetadataModel shipToNum_Metadata { get; set; }

        [CustomDisplayName("PRICELIST_SEQUENCENO")]
        public int? SeqNum { get; set; }
        public TableProfileMetadataModel seqNum_Metadata { get; set; }

        [CustomDisplayName("PRICELIST_CUSTOMERNUM")] // cust Num
        public string custID { get; set; }
        public TableProfileMetadataModel CustNum_Metadata { get; set; }

        [CustomDisplayName("PRICELIST_CUSTOMERGROUP")]
        public string CustGroup { get; set; }
        public TableProfileMetadataModel CustGroup_Metadata { get; set; }




        // public IEnumerable<SelectListItem> lstEmployeeProfile { get; set; }       
    }
    public partial class PriceListMaintenanceModel
    {
        public string record_Id { get; set; }
        public bool readOnly { get; set; }

        public TableProfileModel tableProfile { get; set; }

        public PriceListModel PriceList { get; set; }
        public string test { get; set; }
    }
}