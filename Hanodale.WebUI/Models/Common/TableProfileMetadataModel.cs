using System;
using System.Collections.Generic;
using Hanodale.Domain.DTOs;
using System.ComponentModel.DataAnnotations;
using Hanodale.Utility.Globalize;
using System.Web.Mvc;
using Hanodale.WebUI.Helpers;
using System.ComponentModel;

namespace Hanodale.WebUI.Models
{
    public class TableProfileMetadataModel
    {
        public int id { get; set; }
        public string fieldName { get; set; }
        public string valueFieldName { get; set; }
        public bool isMandatory { get; set; }
        public bool visibilityInGridView { get; set; }
        public bool visibilityInCreate { get; set; }
        public bool isEditableInCreate { get; set; }
        public string mandatoryClass { get; set; }
        public string templateName { get; set; }
        public string resourceKeyNameForGridView { get; set; }
        public string labelNameForGridView { get; set; }
        public string validationRule { get; set; }
        public Nullable<int> sortOrder { get; set; }

        public string selectionOption { get; set; }
        public string defaultValue { get; set; }

        public Nullable<DateTime> minDate { get; set; }
        public Nullable<DateTime> maxDate { get; set; }
        public Nullable<bool> isAllowPastDate { get; set; }
        public Nullable<bool> isFollowCalendarSetting { get; set; }
        public Nullable<DateTime> dateDefaultValue { get; set; }

        public decimal decimalMinLength { get; set; }
        public decimal decimalMaxLength { get; set; }
        public Nullable<decimal> decimalDefaultValue { get; set; }


        public bool isMultipleSelectionDropdown { get; set; }
        public int dropdownModuleType_Id { get; set; }
        public Nullable<int> dropdownDefaultValue { get; set; }
        public bool hasDropdownParent { get; set; }
        public string dropdownParentFieldName { get; set; }
        public string dataRetrieveURL { get; set; }


        public int intMinLength { get; set; }
        public int intMaxLength { get; set; }
        public Nullable<int> intDefaultValue { get; set; }

        public int stringMinLength { get; set; }
        public int stringMaxLength { get; set; }
        public string stringDefaultValue { get; set; }

    }

}