using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class LanguageModel
    {
        public LanguageItemModel defaultLanguage { get; set; }

        public List<LanguageItemModel> lstLanguage { get; set; }

    }

    public class LanguageItemModel
    {
        public string languageName { get; set; }
        public string cultureName { get; set; }
        public bool isDefault { get; set; }
        public bool visibility { get; set; }
        public string flagSymbol { get; set; }
    }

}