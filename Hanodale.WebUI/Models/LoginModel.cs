using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hanodale.WebUI.Models
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "USER_EMAIL", ResourceType = typeof(Resources))]
        //[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Enter a valid email")]
        [StringLength(50, ErrorMessage = "The Maximum length is {1}")]
        public string UserName { get; set; }

        [UIHint("Password")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "USER_PASSWORD", ResourceType = typeof(Resources))]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "The Minimum length is {2} and maximum length is {1} characters")]
        public string Password { get; set; }

        [Display(Name = "USER_REMEMBER_ME", ResourceType = typeof(Resources))]
        public bool RememberMe { get; set; }

        public Nullable<int> totalcurrentRequest { get; set; }

        public Nullable<int> totalNewRequest { get; set; }

        public Nullable<int> totalClosingRequest { get; set; }

        public string  message { get; set; }
        public bool isDigestAuthentication { get; set; }
    }
}