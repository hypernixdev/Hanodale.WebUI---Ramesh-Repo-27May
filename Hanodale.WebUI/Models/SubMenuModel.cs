using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hanodale.WebUI.Models
{
    public class SubMenuModel
    {
        public int id { get; set; }
        public int mainMenu_Id { get; set; }
        public string subMenuName { get; set; }
        public string pageName { get; set; }
        public string pageUrl { get; set; }
        public string imageUrl { get; set; }
        public Nullable<bool> isMainMenu { get; set; }
        public Nullable<int> reportCategory_Id { get; set; }
        public UserRightsModel userRights { get; set; }
    }
}