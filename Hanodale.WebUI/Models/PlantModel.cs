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
    public class PlantModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public AccessRightsModel accessRight { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PLANT_COMPANY")]
        public string company { get; set; }
        public TableProfileMetadataModel company_Metadata { get; set; }


        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PLANT_NAME")]
        public string name { get; set; }
        public TableProfileMetadataModel name_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PLANT_CODE")]
        public string plant { get; set; }
        public TableProfileMetadataModel plant_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PLANT_ADDRESS1")]
        public string address1 { get; set; }
        public TableProfileMetadataModel address1_Metadata { get; set; }

        [CustomDisplayName("PLANT_ADDRESS2")]
        public string address2 { get; set; }
        public TableProfileMetadataModel address2_Metadata { get; set; }

        [CustomDisplayName("PLANT_ADDRESS3")]
        public string address3 { get; set; }
        public TableProfileMetadataModel address3_Metadata { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("PLANT_CITY")]
        public string city { get; set; }
        public TableProfileMetadataModel city_Metadata { get; set; }

        [CustomDisplayName("PLANT_STATE")]
        public string state { get; set; }
        public TableProfileMetadataModel state_Metadata { get; set; }

        [CustomDisplayName("PLANT_ZIP")]
        public string zip { get; set; }
        public TableProfileMetadataModel zip_Metadata { get; set; }

        // Additional properties for Plant model
        // You may add other properties as needed
    }
}