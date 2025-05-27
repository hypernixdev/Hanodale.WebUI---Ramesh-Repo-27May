using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hanodale.WebUI.Models
{
    public partial class FileHistoryViewModel
    {
        public string id { get; set; }

        public bool isEdit { get; set; }

        public bool readOnly { get; set; }


        public int uploadType { get; set; }
        [Display(Name = "FILEHISTORY_FILENAME", ResourceType = typeof(Resources))]
        public string fileName { get; set; }
        [UIHint("Number")]
        [Display(Name = "FILEHISTORY_TOTALRECORDS", ResourceType = typeof(Resources))]
        public int totalRecords { get; set; }
        public int user_Id { get; set; }
        public int subCostId { get; set; }
        public int mainCostId { get; set; }
        [Display(Name = "FILEHISTORY_UPLOADEDBY", ResourceType = typeof(Resources))]
        public string createdBy { get; set; }
        [Display(Name = "FILEHISTORY_UPLOADEDDATE", ResourceType = typeof(Resources))]
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }

    }
    public partial class FileHistoryViewModel
    {
        public List<FileUploadHistorys> lstFileHistoryModel { get; set; }
    }
}