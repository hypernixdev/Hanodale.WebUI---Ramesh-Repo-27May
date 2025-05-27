using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class MetadataModel
    {
        public int tableProfileId { get; set; }
        public string fieldName { get; set; }
        public bool isMandatory { get; set; }
        public bool visibilityInEdit { get; set; }
        public bool visibilityInGridView { get; set; }
        public bool visibilityInCreate { get; set; }
        public bool isEditableInCreate { get; set; }
        public bool isEditableInEdit { get; set; }
        public string translatedLabel { get; set; }
    }
}