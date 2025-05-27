using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class UserRightsModel
    {
        public int id { get; set; }
        [UIHint("ComboBoxSearchable")]
        [Required]
        [Display(Name = "USER_ROLE", ResourceType = typeof(Resources))]
        public int userRole_Id { get; set; }
        public int subMenu_Id { get; set; }
        [UIHint("HBool")]
        [Required]
        public bool canView { get; set; }
        [UIHint("HBool")]
        [Required]
        public bool canAdd { get; set; }
        [UIHint("HBool")]
        [Required]
        public bool canEdit { get; set; }
        [UIHint("HBool")]
        [Required]
        public bool canDelete { get; set; }

        [UIHint("HBool")]
        [Required]
        public bool allCheck { get { return (this.canAdd && this.canView && this.canEdit && this.canDelete); } }

        public string createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }

        //public SubMenu SubMenu { get; set; }
        //public UserRole UserRole { get; set; }

        public List<MainMenuModel> mainMenu { get; set; }


        public IEnumerable<SelectListItem> UserRoles { get; set; }
    }
}