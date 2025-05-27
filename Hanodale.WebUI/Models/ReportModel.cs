using System;
using System.Collections.Generic;
using Hanodale.Domain.DTOs;

namespace Hanodale.WebUI.Models
{
    public partial class ReportModel
    {
        public int id { get; set; }
        public int organization_Id { get; set; }
        public int? parent_Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
        public string fontColor { get; set; }
        public string backColor { get; set; }
        public int ordering { get; set; }
        public bool visibility { get; set; }
        public string issue { get; set; }
        public string rev { get; set; }
        public string tag { get; set; }
        public List<ReportModel> ChildList { get; set; }
    }
   
}