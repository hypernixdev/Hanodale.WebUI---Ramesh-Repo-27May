using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class BusinessClassificationModel
    {
        public int businessclassificationID { get; set; }

        public int business_Id { get; set; }

        public int classification_Id { get; set; }

        public string createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public System.DateTime modifiedDate { get; set; }

        public List<BusinessClassificationItemViewModel> lstClassificationItem = new List<BusinessClassificationItemViewModel>();

        public int[] classification_Ids { get; set; }
    }

    public partial class BusinessClassificationItemViewModel 
    {
        public int moduleItem_Id { get; set; }

        public string moduleItemName { get; set; }

        public bool isCheck { get; set; }
    }
}