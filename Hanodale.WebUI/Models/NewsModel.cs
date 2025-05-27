using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class NewsModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public bool isBack { get; set; }

        [UIHint("TextArea")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "NEWS_DESCRIPTION", ResourceType = typeof(Resources))]
        [StringLength(500, ErrorMessage = "The Maximum length is {1}")]
        public string description { get; set; }

        [Display(Name = "NEWS_LOGGED_BY", ResourceType = typeof(Resources))]
        [StringLength(20, ErrorMessage = "The Maximum length is {1}")]
        public string loggedBy { get; set; }

        public Nullable<System.DateTime> loggedDate { get; set; }

        public string createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }
    }
    public partial class NewsViewModel
    {
        public RecordDetails recordDetails { get; set; }

        public List<NewsModel> lstNewsModel { get; set; }
    }
}