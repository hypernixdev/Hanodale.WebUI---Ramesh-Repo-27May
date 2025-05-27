using System;
using System.Collections.Generic;
using Hanodale.Domain.DTOs;
using System.ComponentModel.DataAnnotations;
using Hanodale.Utility.Globalize;
using System.Web.Mvc;
using Hanodale.WebUI.Helpers;
using System.ComponentModel;

namespace Hanodale.WebUI.Models
{
    public class GridViewIndexModel
    {
        public string id { get; set; }

        public string masterRecord_Id { get; set; }

        public int gridViewButtonColumnWidth { get; set; }
        public int columnCount { get; set; }

        public bool readOnly { get; set; }

        public AccessRightsModel accessRight { get; set; }

        public List<GridViewColumnModel> lstColumn { get; set; }

        [UIHint("FileBrowser")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "UPLOAD_FILE_URLPATH", ResourceType = typeof(Resources))]
        public string file { get; set; }

    }
    public partial class GridViewColumnModel
    {
        public string resourceKeyName { get; set; }
        public string fieldName { get; set; }
        public int sortOrder { get; set; }
        public bool isExpandable { get; set; }
    }

}