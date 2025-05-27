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
    public class StockBalanceModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public AccessRightsModel accessRight { get; set; }
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("STOCKBALANCE_COMPANY")]
        public string company { get; set; }
        public TableProfileMetadataModel company_Metadata { get; set; }

        
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("STOCKBALANCE_PARTNUM")]
        public string partNum { get; set; }
        public TableProfileMetadataModel partNum_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("STOCKBALANCE_WAREHOUSECODE")]
        public string warehouseCode { get; set; }
        public TableProfileMetadataModel warehouseCode_Metadata { get; set; }
       
        [UIHint("Decimal")]
       // [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("STOCKBALANCE_ONHANDQTY")]
        public decimal onHandQty { get; set; }
        public TableProfileMetadataModel onHandQty_Metadata { get; set; }

  
        //[Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("STOCKBALANCE_UOM")]
        public string uom { get; set; }

        public TableProfileMetadataModel uom_Metadata { get; set; }

        [CustomDisplayName("STOCKBALANCE_UNIQUEFIELD")]
        public string uniqueField { get; set; }

        public TableProfileMetadataModel uniqueField_Metadata { get; set; }
        public IEnumerable<SelectListItem> lstProduct { get; set; }

        [CustomDisplayName("STOCKBALANCE_LOCATION")]
        public string location { get; set; }
        public TableProfileMetadataModel location_Metadata { get; set; }

        // Additional properties for StockBalance model
        // You may add other properties as needed
    }
}