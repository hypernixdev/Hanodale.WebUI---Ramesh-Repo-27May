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
    public class SchedulerLogModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }

        public AccessRightsModel accessRight { get; set; }
      
        [CustomDisplayName("SCHEDULERLOG_SCHEDULERSETTINGID")]
        public int schedulerSetting_Id { get; set; }
        public TableProfileMetadataModel schedulerSetting_Id_Metadata { get; set; }

         [CustomDisplayName("SCHEDULERLOG_STARTDATETIME")]
        public Nullable<System.DateTime> startDateTime { get; set; }
        public TableProfileMetadataModel startDateTime_Metadata { get; set; }

         [CustomDisplayName("SCHEDULERLOG_ENDDATETIME")]
        public Nullable<System.DateTime> endDateTime { get; set; }
        public TableProfileMetadataModel endDateTime_Metadata { get; set; }

         [CustomDisplayName("SCHEDULERLOG_RESULT")]
        public Nullable<bool> result { get; set; }

        [CustomDisplayName("SCHEDULERLOG_TOTALRECORDPROCESSED")]
        public int totalRecordProcessed { get; set; }


        [CustomDisplayName("SCHEDULERLOG_ERRORMESSAGE")]
        public string errorMessage { get; set; }


        public TableProfileMetadataModel result_Metadata { get; set; }

       public IEnumerable<SelectListItem> lstsyncModule { get; set; }
        public IEnumerable<SelectListItem> lsttimeSlot { get; set; }

        
    }
  
}