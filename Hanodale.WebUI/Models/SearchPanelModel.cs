using Hanodale.Domain.DTOs;
using Hanodale.Utility.Globalize;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hanodale.WebUI.Models
{
    public class SearchPanelModel
    {
        [Display(Name = "STOCKGROUP_CODE", ResourceType = typeof(Resources))]
        //[StringLength(50, ErrorMessage = "The Maximum length is {1} characters")]
        public string code { get; set; }

        [UIHint("TextArea")]
        [Display(Name = "STOCKGROUP_DESCRIPTION", ResourceType = typeof(Resources))]
        //[StringLength(500, ErrorMessage = "The Maximum length is {1} characters")]
        public string description { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Display(Name = "STOCK_STOCKGROUPID", ResourceType = typeof(Resources))]
        public Nullable<int> stockGroup_Id { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "RFQ_CREATED_DATE_FROM", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> createdDateFrom { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy h:mm:ss tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "RFQ_CREATED_DATE_TO", ResourceType = typeof(Resources))]
        public Nullable<System.DateTime> createdDateTo { get; set; }

        [UIHint("TextArea")]
        [Display(Name = "STOCK_PARTNAME", ResourceType = typeof(Resources))]
        //[StringLength(500, ErrorMessage = "The Maximum length is {1} characters")]
        public string partName { get; set; }

        public int searchType { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Display(Name = "ASSETTYPEMASTERCOMP_ASSETTYPE_ID", ResourceType = typeof(Resources))]
        public int assetTypeId { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ACTIVE_STATUS", ResourceType = typeof(Resources))]
        public int status { get; set; }

        [UIHint("ComboBoxSearchable")]
        [Required(ErrorMessageResourceName = "RequiredSelect", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ACTIVE_STATUS", ResourceType = typeof(Resources))]
        public string ActiveStatus { get; set; }

        public IEnumerable<SelectListItem> lstStockGroup { get; set; }
        public IEnumerable<SelectListItem> lstAssetType { get; set; }
        public IEnumerable<SelectListItem> lstStatus { get; set; }
        public IEnumerable<SelectListItem> lstAsset { get; set; }
    }

}