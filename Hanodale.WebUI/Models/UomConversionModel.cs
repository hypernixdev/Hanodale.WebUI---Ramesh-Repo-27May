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
    public class UomConversionModel
    {
        public string id { get; set; }

        public string partNumId { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public AccessRightsModel accessRight { get; set; }




        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("UOMCOVERSION_COMPANY")]
        [StringLength(100, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
        public string company { get; set; }

        public TableProfileMetadataModel company_Metadata { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
       // [CustomDisplayName("UOMCOVERSION_PARTNUM")]
      //  [StringLength(100, ErrorMessageResourceName = "MaxStringLength", ErrorMessageResourceType = typeof(Resources))]
      //  public string partNum { get; set; }

       // public TableProfileMetadataModel partNum_Metadata { get; set; }


        // [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("UOMCOVERSION_UOMCODE")]
        public string uomCode { get; set; }

        public TableProfileMetadataModel uomCode_Metadata { get; set; }


        // [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("UOMCOVERSION_CONVFACTOR")]
        public string convFactor { get; set; }

        public TableProfileMetadataModel convFactor_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("UOMCOVERSION_UNIQUEFIELD")]
        public string uniqueField { get; set; }

        public TableProfileMetadataModel uniqueField_Metadata { get; set; }

       

        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("UOMCOVERSION_CONVOPERATOR")]
        public string convOperator { get; set; }

        public TableProfileMetadataModel convOperator_Metadata { get; set; }


    }

}