using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Hanodale.WebUI.Models
{
    public class AccessRightsModel
    {
        public bool canView { get; set; }
        public bool canAdd { get; set; }
        public bool canEdit { get; set; }
        public bool canDelete { get; set; }
        public string pageName { get; set; }
        public bool readOnly { get; set; }
        public int pageId { get; set; }

        public string elementId { get; set; }
        public object param { get; set; }
        public bool inout { get; set; }
        public bool rfqOpenned { get; set; }
        public int organizationId { get; set; }
        public int[] workorderIds { get; set; }
        public string userRoleIds { get; set; }
        //public DashboardInfo DashboardInfo { get; set; } 
    }

}