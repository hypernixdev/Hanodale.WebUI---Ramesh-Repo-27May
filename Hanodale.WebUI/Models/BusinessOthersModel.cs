using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class BusinessOthersModel
    {
        public string businessOtherID { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public string business_Id { get; set; }

        public int businessId { get; set; }

        [Display(Name = "BUSINESSOTHERS_BUMISHARE", ResourceType = typeof(Resources))]
        public Nullable<decimal> bumiShare { get; set; }

        [Display(Name = "BUSINESSOTHERS_NONBUMISHARE", ResourceType = typeof(Resources))]
        public Nullable<decimal> nonBumiShare { get; set; }

        [Display(Name = "BUSINESSOTHERS_FOREGINSHARE", ResourceType = typeof(Resources))]
        public Nullable<decimal> foreignShare { get; set; }

        [Display(Name = "BUSINESSOTHERS_BUMICAPITAL", ResourceType = typeof(Resources))]
        public Nullable<decimal> bumiCapital { get; set; }

        public Nullable<bool> classA { get; set; }

        public Nullable<bool> classB { get; set; }

        public Nullable<bool> classBX { get; set; }

        public Nullable<bool> classC { get; set; }

        public Nullable<bool> classD { get; set; }

        public Nullable<bool> classE { get; set; }

        public Nullable<bool> classEX { get; set; }

        public Nullable<bool> classF { get; set; }

        public Nullable<bool> pkk { get; set; }

        public Nullable<bool> tnb { get; set; }

        public Nullable<bool> jba { get; set; }

        public Nullable<bool> maras { get; set; }

        public Nullable<bool> dbkl { get; set; }

        public Nullable<bool> financeMinistry { get; set; }

        public Nullable<bool> jkh { get; set; }

        public Nullable<bool> jkrs { get; set; }

        public string createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }

        //Business
        [Display(Name = "BUSINESS_BUSINESSCATEGORY", ResourceType = typeof(Resources))]
        [StringLength(200, ErrorMessage = "The Maximum length is {1}")]
        public string businessCategory { get; set; }

        [UIHint("Number")]
        [Display(Name = "BUSINESS_PAIDUPCAPTIAL", ResourceType = typeof(Resources))]
        public Nullable<int> paidUpCapital { get; set; }
        //

    }
    public partial class BusinessOthersViewModel
    {
        public List<BusinessOthersModel> lstBusinessOthersModel { get; set; }
    }
}