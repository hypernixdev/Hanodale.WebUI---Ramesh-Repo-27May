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
    public class MasterProfileModel
    {
        public string record_Id { get; set; }
        public bool readOnly { get; set; }

        public TableProfileModel tableProfile { get; set; }
    }

    public class TableProfileModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public Nullable<int> menu_Id { get; set; }
        public Nullable<int> parent_Id { get; set; }
        public string url { get; set; }
        public string controllerName { get; set; }
        public string actionName { get; set; }
        public int localizationResource_Id { get; set; }
        public string resourceNameKey { get; set; }
        public string icon { get; set; }
        public int sortOrder { get; set; }
        public bool visibility { get; set; }

        public List<TableProfileModel> lstTableProfileTab { get; set; }

    }

}