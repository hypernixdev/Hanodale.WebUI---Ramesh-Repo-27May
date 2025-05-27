using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class BusinessAddressModel
    {
        public int businessaddressID { get; set; }

        public string business_Id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        //[UIHint("ComboBox")]
        //[Display(Name = "BUSINESS_CLASSIFICATION_CLASSIFICATION", ResourceType = typeof(Resources))]
        //public int temprary_Id { get; set; }

        [UIHint("TextArea")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESSADDRESS_ADDRESS", ResourceType = typeof(Resources))]
        [StringLength(150, ErrorMessage = "The Maximum length is {1}")]
        public string address { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESSADDRESS_CITY", ResourceType = typeof(Resources))]
        [StringLength(20, ErrorMessage = "The Maximum length is {1}")]
        public string city { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESSADDRESS_PROVINCE", ResourceType = typeof(Resources))]
        [StringLength(20, ErrorMessage = "The Maximum length is {1}")]
        public string province { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESSADDRESS_POSTAL_CODE", ResourceType = typeof(Resources))]
        [StringLength(20, ErrorMessage = "The Maximum length is {1}")]
        public string postalCode { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESSADDRESS_COUNTRY", ResourceType = typeof(Resources))]
        [StringLength(20, ErrorMessage = "The Maximum length is {1}")]
        public string country { get; set; }

        public string createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }


        // Model for Classificaiton table 

        public int businessclassificationID { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Display(Name = "BUSINESS_CLASSIFICATION_CLASSIFICATION", ResourceType = typeof(Resources))]
        public int classification_Id { get; set; }

        //public List<BusinessAddressViewModel> lstClassificationItem = new List<BusinessAddressViewModel>();

        [UIHint("ComboBoxSearchable")]
        public IEnumerable<SelectListItem> lstClassificationItem { get; set; }
        public int[] classification_Ids { get; set; }

    }

    public partial class BusinessAddressViewModel
    {
        public List<BusinessAddressModel> lstBusinessAddress { get; set; }

        // Model for Classificaiton table 

        public int moduleItem_Id { get; set; }

        public string moduleItemName { get; set; }

        public bool isCheck { get; set; }
    }

}