using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class UserNotificationModel
    {
        public string id { get; set; }

        public string date { get; set; }

        public string title { get; set; }

        public string message { get; set; }
    }
}