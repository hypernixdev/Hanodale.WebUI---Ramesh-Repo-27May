using System;
using System.Collections.Generic;
using Hanodale.Domain.DTOs;
using System.ComponentModel.DataAnnotations;
using Hanodale.Utility.Globalize;
using System.Web.Mvc;
using Hanodale.WebUI.Helpers;
using System.ComponentModel;
using Hanodale.BusinessLogic;
using Microsoft.Practices.ServiceLocation;

namespace Hanodale.WebUI.Models
{
    public class PriceListPartModel
    {
        public string id { get; set; }

        public string uomCode { get; set; }

        public string partNum { get; set; }
        public string listCode { get; set; }

        public decimal basePrice { get; set; }
    }
   
}