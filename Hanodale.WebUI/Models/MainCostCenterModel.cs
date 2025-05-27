using System;
using System.Collections.Generic;
using Hanodale.Domain.DTOs;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hanodale.WebUI.Models
{
    public class MainCostCenterModel : Organizations
    {
        [UIHint("ComboBox")]
        public int mainCostCenter_Id { get; set; }
        public IEnumerable<SelectListItem> MainCostCenter { get; set; }

        [UIHint("ComboBox")]
        public int organization_Id { get; set; }
        public IEnumerable<SelectListItem> SubCostCenter { get; set; }
    }
}