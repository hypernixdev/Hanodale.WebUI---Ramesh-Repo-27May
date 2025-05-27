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
    public class LooseConversionModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public AccessRightsModel accessRight { get; set; }
        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("LOOSECOVERSION_PRODUCTCARTON")]

        public int productCarton_Id { get; set; }    
        public TableProfileMetadataModel productCarton_Id_Metadata { get; set; }

        
        //[Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTCARTON_BARCODE")]
        public string barcode { get; set; }
        public TableProfileMetadataModel barcode_Metadata { get; set; }

        [CustomDisplayName("PRODUCTCARTON_EPICODE_PART_NO")]
        public string epicorPartNo { get; set; }
        public TableProfileMetadataModel epicorPartNo_Metadata { get; set; }

        public DateTime createdDate { get; set; }

        public string createdBy { get; set; }
        public string LooseConv { get; set; }


        public IEnumerable<SelectListItem> lstProduct { get; set; }

        public List<LooseConversionItemsModel> looseItems { get; set; }


        // Additional properties for LooseConversion model
        // You may add other properties as needed
    }
    public class LooseConversionItemsModel
    {
        public int Id { get; set; }
        public string LooseBarcode { get; set; }
        public decimal LooseQty { get; set; }
        public decimal RunningBalance { get; set; }
        public int LooseConversionId { get; set; }
        public int weighScaleBarcode_Id { get; set; }


        
        public virtual LooseConversion LooseConversion { get; set; }

    }
}