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
    public class ProductWeightBarcodeModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public AccessRightsModel accessRight { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTWEIGHTBARCODE_EPICOREPARTNO")]
        public string epicorePartNo { get; set; }
        public TableProfileMetadataModel epicorePartNo_Metadata { get; set; }

        
        [CustomDisplayName("PRODUCTWEIGHTBARCODE_DESCRIPTION")]
        public string description { get; set; }
        public TableProfileMetadataModel description_Metadata { get; set; }

        [CustomDisplayName("PRODUCTWEIGHTBARCODE_FULLBARCODE")]
        public string fullBarcode { get; set; }
        public TableProfileMetadataModel fullBarcode_Metadata { get; set; }

       // [UIHint("Decimal")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTWEIGHTBARCODE_BARCODE")]
        public string barcode { get; set; }
        public TableProfileMetadataModel barcode_Metadata { get; set; }
        public string barcode1 { get; set; }

        //[UIHint("Decimal")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTWEIGHTBARCODE_BARCODEFROMPOS")]
        public int barcodeFromPos { get; set; }
        public TableProfileMetadataModel barcodeFromPos_Metadata { get; set; }

        //[UIHint("Decimal")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTWEIGHTBARCODE_BARCODETOPOS")]
        public int barcodeToPos { get; set; }
        public TableProfileMetadataModel barcodeToPos_Metadata { get; set; }

        //[UIHint("Decimal")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTWEIGHTBARCODE_WEIGHTFROMPOS")]
        public int weightFromPos { get; set; }
        public TableProfileMetadataModel weightFromPos_Metadata { get; set; }

        //[UIHint("Decimal")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTWEIGHTBARCODE_WEIGHTTOPOS")]
        public int weightToPos { get; set; }
        public TableProfileMetadataModel weightToPos_Metadata { get; set; }

        [UIHint("Decimal")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTWEIGHTBARCODE_WEIGHTMULTIPLY")]
        public Nullable<decimal> weightMultiply { get; set; }
        public TableProfileMetadataModel weightMultiply_Metadata { get; set; }
      
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTWEIGHTBARCODE_BARCODELENGTH")]
        public int barcodeLength { get; set; }
        public TableProfileMetadataModel barcodeLength_Metadata { get; set; }

         
        //[Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTWEIGHTBARCODE_OFFSET1")]
        public string offSet1 { get; set; }
        public TableProfileMetadataModel offSet1_Metadata { get; set; }

         
      //  [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTWEIGHTBARCODE_OFFSET2")]
        public string offSet2 { get; set; }
        public TableProfileMetadataModel offSet2_Metadata { get; set; }


         
      //  [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PRODUCTWEIGHTBARCODE_WEIGHTVALUE")]
        public string weightValue { get; set; }
        public string weightValue1 { get; set; }

        public TableProfileMetadataModel weightValue_Metadata { get; set; }

        public IEnumerable<SelectListItem> lstProduct { get; set; }


        // Additional properties for ProductWeightBarcode model
        // You may add other properties as needed
    }
}