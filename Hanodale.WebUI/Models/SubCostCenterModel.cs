using System;
using System.Collections.Generic;
using Hanodale.Domain.DTOs;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class SubCostCenterModel : Organizations
    {
        public int organization_Id  { get; set; }
        public IEnumerable<SelectListItem> SubCostCenter { get; set; }
    }
}