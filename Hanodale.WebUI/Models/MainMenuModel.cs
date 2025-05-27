using Hanodale.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hanodale.WebUI.Models
{
    public class MainMenuModel
    {
        public int id { get; set; }
        public string menuName { get; set; }
        public string pageName { get; set; }
        public string pageUrl { get; set; }
        public string imageUrl { get; set; }
        public string status { get; set; }
        public List<SubMenuModel> subMenus { get; set; }
    }
}