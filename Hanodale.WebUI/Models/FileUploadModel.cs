using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hanodale.WebUI.Models
{
    public class FileUploadModel
    {
        [UIHint("FileBrowser")]
        [Display(Name = "File Name")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        public string FileName { get; set; }

        public bool isAdd { get; set; }
    }
}