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
    /// <summary>
    /// Class that encapsulates most common parameters sent by DataTables plugin
    /// </summary>
    public class DashboardModel
    {
        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "WORKORDER_ESTIMATEDSTARTDATE", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> estimatedStartDate { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "WORKORDER_ESTIMATEDENDDATE", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> estimatedEndDate { get; set; }

        //[UIHint("DateWitMonth")]
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        //[Display(Name = "WO_ESTIMATE_START_DATE_FROM", ResourceType = typeof(Resources))]
        //public Nullable<System.DateTime> estimatedStartDateFrom { get; set; }

        //[UIHint("DateWitMonth")]
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        //[Display(Name = "WO_ESTIMATE_START_DATE_TO", ResourceType = typeof(Resources))]
        //public Nullable<System.DateTime> estimatedEndDateTo { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        // [Display(Name = "WO_ESTIMATE_START_DATE_FROM", ResourceType = typeof(Resources))]
        [Display(Name = "WO_RECIEVED_START_DATE_FROM", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> estimatedStartDateFrom { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        //[Display(Name = "WO_ESTIMATE_START_DATE_TO", ResourceType = typeof(Resources))]
        [Display(Name = "WO_RECIEVED_END_DATE_TO", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> estimatedEndDateTo { get; set; }

        // [UIHint("DateWitMonth")]
        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "RFQ_CREATED_DATE_FROM", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> createdDateFrom { get; set; }

        // [UIHint("DateWitMonth")]
        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "RFQ_CREATED_DATE_TO", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> createdDateTo { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "RFQ_CREATED_DATE_FROM", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> createdFrom { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "RFQ_CREATED_DATE_TO", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> createdTo { get; set; }

        //[UIHint("DateWitMonth")]
        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "SCHEDULED_NEXTSERVICEDATE", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> nextServiceDate { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "SCHEDULED_NEXTSERVICEDATE", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> nextServiceDateTo { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Display(Name = "WORKORDER_MAINTANANCETYPE_ID", ResourceType = typeof(Resources))]
        public Nullable<int> maintenanceType_Id { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Display(Name = "PURCHASEREQUEST_PURCHASEREQUESTTYPE", ResourceType = typeof(Resources))]
        public Nullable<int>[] PurchaseRequestType_Id { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Display(Name = "RFQMASTER_RFQSTATUS", ResourceType = typeof(Resources))]
        public Nullable<int> RFQStatus_Id { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Display(Name = "FILTERBY", ResourceType = typeof(Resources))]
        public Nullable<int> filterIds { get; set; }

        public List<DashboardItems> lst { get; set; }

        public IEnumerable<SelectListItem> lstMaintananceType { get; set; }

        public IEnumerable<SelectListItem> lstPurchaseRequestType { get; set; }

        public IEnumerable<SelectListItem> lstRFQstatus { get; set; }

        public IEnumerable<SelectListItem> lstFilters { get; set; }

        public bool check { get; set; }
        public string element_Id { get; set; }
        public bool isMytask { get; set; }
        public int ticketsCount { get; set; }
        public string dashboardBoxName { get; set; }

        public decimal TotalSales { get; set; }

        public decimal TotalRefund { get; set; }
    }
}