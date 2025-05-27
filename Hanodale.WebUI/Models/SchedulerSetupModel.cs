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
    public class SchedulerSetupModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public AccessRightsModel accessRight { get; set; }
        [UIHint("ComboBox")]

        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("SCHEDULERSETUP_SYNCMODULE")]
        public int syncModule_Id { get; set; }
        public TableProfileMetadataModel syncModule_Id_Metadata { get; set; }

        [UIHint("Date")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("SCHEDULERSETUP_STARTDATE")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        public Nullable<System.DateTime> startDate { get; set; }
        public TableProfileMetadataModel startDate_Metadata { get; set; }

         [UIHint("ComboBox")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("SCHEDULERSETUP_TIMESLOT")]
        public int timeSlot { get; set; }
        public TableProfileMetadataModel timeSlot_Metadata { get; set; }

        [UIHint("HBool")]
        [Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        [CustomDisplayName("SCHEDULERSETUP_ISACTIVE")]
        public Nullable<bool> isActive { get; set; }

        public TableProfileMetadataModel isActive_Metadata { get; set; }

        //[Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        //[CustomDisplayName("SCHEDULERSETUP_CREATEDBY")]
        //public string createdBy { get; set; }
        //public TableProfileMetadataModel createdBy_Metadata { get; set; }

       

        //[Required(ErrorMessageResourceName = "RequiredInput", ErrorMessageResourceType = typeof(Resources))]
        //[CustomDisplayName("SCHEDULERSETUP_CREATEDDATE")]
        //public System.DateTime createdDate { get; set; }
        //public TableProfileMetadataModel createdDate_Metadata { get; set; }

        public IEnumerable<SelectListItem> lstsyncModule { get; set; }
        public IEnumerable<SelectListItem> lsttimeSlot { get; set; }
       
    }
    public partial class SchedulerSetupMaintenanceModel
    {
        public string record_Id { get; set; }
        public bool readOnly { get; set; }

        public TableProfileModel tableProfile { get; set; }

        public SchedulerSetupModel SchedulerSetup { get; set; }
    }
}