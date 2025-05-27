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
    public class CustomerPriceListModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public AccessRightsModel accessRight { get; set; }

        public Nullable<int> CustNum { get; set; }
        public string ShipToNum { get; set; }
        public Nullable<int> SeqNum { get; set; }
        public string ListCode { get; set; }
     
        public string CurrencyCode { get; set; }
       
        public string ListDescription { get; set; }
      
        public DateTime? StartDate { get; set; }
       
        public DateTime? EndDate { get; set; }

    }
    public partial class CustomerPriceListMaintenanceModel
    {
        public string record_Id { get; set; }
        public bool readOnly { get; set; }

        public TableProfileModel tableProfile { get; set; }

        public CustomerPriceListModel CustomerPriceList { get; set; }
        public string test { get; set; }
    }
}