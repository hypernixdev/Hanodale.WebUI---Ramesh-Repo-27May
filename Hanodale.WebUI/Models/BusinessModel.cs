using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{

    public class BusinessModel
    {
        public string id { get; set; }

        public string business_ID { get; set; }

        public bool issupplier { get; set; }

        public bool isprofileupdate { get; set; }

        public bool isbusinessType_Id { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESS_BUSINESS_TYPE", ResourceType = typeof(Resources))]
        public int businessType_Id { get; set; }
        public string businessType_Id_Label { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Display(Name = "WORKCATEGORY_ID", ResourceType = typeof(Resources))]
        public int WorkCateoryId { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESS_NAME", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1}")]
        public string name { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESS_CODE", ResourceType = typeof(Emails))]
        [StringLength(20, ErrorMessage = "The Maximum length is {1}")]
        public string code { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESS_PHONE", ResourceType = typeof(Resources))]
        [StringLength(14, ErrorMessage = "The Maximum length is {1}")]
        public string phone { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESS_PHONE2", ResourceType = typeof(Resources))]
        [StringLength(14, ErrorMessage = "The Maximum length is {1}")]
        public string phone2 { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESS_FAX", ResourceType = typeof(Resources))]
        [StringLength(14, ErrorMessage = "The Maximum length is {1}")]
        public string fax { get; set; }

        [Display(Name = "BUSINESS_WEBSITE", ResourceType = typeof(Resources))]
        [StringLength(50, ErrorMessage = "The Maximum length is {1}")]
        public string webSite { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESS_PRIMARY_CONTACT", ResourceType = typeof(Resources))]
        [StringLength(55, ErrorMessage = "The Maximum length is {1}")]
        public string primaryContact { get; set; }

        [UIHint("TextArea")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESS_PRIMARY_EMAIL", ResourceType = typeof(Resources))]
        //[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Enter a valid email")]
        [StringLength(1000, ErrorMessage = "The Maximum length is {1}")]
        public string primaryEmail { get; set; }

        [UIHint("ComboBox")]
        [Display(Name = "BUSINESS_PRIMARY_CURRENCY", ResourceType = typeof(Resources))]
        public string primaryCurrency { get; set; }

        [UIHint("TextArea")]
        [Display(Name = "BUSINESS_REMARKS", ResourceType = typeof(Resources))]
        [StringLength(500, ErrorMessage = "The Maximum length is {1}")]
        public string remarks { get; set; }

        [UIHint("DateTime")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "BUSINESS_PROFILELASTUPDATED", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> profileLastUpdated { get; set; }

        [Display(Name = "BUSINESS_GSTNUMBER", ResourceType = typeof(Resources))]
        [StringLength(20, ErrorMessage = "The Maximum length is {1}")]
        public string gstNo { get; set; }

        public string createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }


        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ACTIVE_STATUS", ResourceType = typeof(Resources))]
        public string ActiveStatus { get; set; }


        [UIHint("HBool")]
        //[Display(Name = "BUSINESS_STATUS", ResourceType = typeof(Resources))]
        public bool status { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public IEnumerable<SelectListItem> lstprimaryCurrency { get; set; }

        public IEnumerable<SelectListItem> BusinessTypes { get; set; }
        public IEnumerable<SelectListItem> lstStatus { get; set; }

        // Model for Classificaiton table 

        //[UIHint("MultiSelectComboBox")]
        ////[AdditionalMetadata("class", "multipleSelect")]
        //[Display(Name = "BUSINESS_CLASSIFICATION_CLASSIFICATION", ResourceType = typeof(Resources))]
        //public int classifiy_Id { get; set; }

        //public List<BusinessAddressViewModel> lstClassificationItem = new List<BusinessAddressViewModel>();

        public IEnumerable<SelectListItem> lstWorkCategory { get; set; }

        public IEnumerable<SelectListItem> lstClassificationItem { get; set; }

        [UIHint("MultiSelectComboBox")]
        [Display(Name = "BUSINESS_CLASSIFICATION_CLASSIFICATION", ResourceType = typeof(Resources))]
        public int[] classification_Ids { get; set; }

        public List<WorkCategorys> WorkCategoryList { get; set; }

        public IEnumerable<SelectListItem> lstCompanyType { get; set; }

        public int[] workCategoryIds { get; set; }

        public bool isSelected { get; set; }

        [UIHint("HBool")]
        [Display(Name = "BUSINESS_ISMAHBREGISTERED", ResourceType = typeof(Resources))]
        public Nullable<bool> isMAHBRegistered { get; set; }

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "BUSINESS_REGISTRATIONNO", ResourceType = typeof(Resources))]
        [StringLength(100, ErrorMessage = "The Maximum length is {1}")]
        public string registrationNo { get; set; }

        [UIHint("Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "BUSINESS_REGISTRATIONDATE", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> registrationDate { get; set; }

        [UIHint("Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "BUSINESS_EXPIRYDATE", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> expiryDate { get; set; }

        [Display(Name = "BUSINESS_REFERENCEID", ResourceType = typeof(Resources))]
        [StringLength(100, ErrorMessage = "The Maximum length is {1}")]
        public string referenceId { get; set; }

        [Display(Name = "BUSINESS_ROCNO", ResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(100, ErrorMessage = "The Maximum length is {1}")]
        public string rocNo { get; set; }

        [Display(Name = "BUSINESS_BUSINESSCATEGORY", ResourceType = typeof(Resources))]
        [StringLength(200, ErrorMessage = "The Maximum length is {1}")]
        public string businessCategory { get; set; }

        [Display(Name = "BUSINESS_PAIDUPCAPTIAL", ResourceType = typeof(Resources))]
        [Range(0, 100000, ErrorMessage = "Please enter valid integer Number")]
        [DataAnnotationsExtensions.Integer(ErrorMessage = "Please enter a valid number.")]
        public Nullable<int> paidUpCapital { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Display(Name = "BUSINESS_COMPANYTYPE", ResourceType = typeof(Resources))]
        public Nullable<int> companyType { get; set; }

        [UIHint("TextArea")]

        [Display(Name = "BUSINESS_ADDRESS", ResourceType = typeof(Resources))]
        [StringLength(1000, ErrorMessage = "The Maximum length is {1}")]
        public string address { get; set; }

        [UIHint("TextArea")]
        [Display(Name = "BUSINESS_LIMITATION", ResourceType = typeof(Resources))]
        [StringLength(500, ErrorMessage = "The Maximum length is {1}")]
        public string limitation { get; set; }

        public List<Organizations> lstorganization { get; set; }

        public List<BusinessCategories> LegendBusinessCategory { get; set; }

        public int[] organisationIds { get; set; }

        public Nullable<int> searchType { get; set; }

    }

    public partial class BusinessViewModel
    {
        public List<BusinessModel> lstBusiness { get; set; }

        // Model for Classificaiton table 

        public int moduleItem_Id { get; set; }

        public string moduleItemName { get; set; }

        public bool isCheck { get; set; }
    }

}