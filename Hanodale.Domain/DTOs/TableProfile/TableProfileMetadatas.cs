using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class TableProfileMetadatas
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int tableProfile_Id { get; set; }
        [DataMember]
        public string fieldName { get; set; }
        [DataMember]
        public string valueFieldName { get; set; }
        [DataMember]
        public string resourceKeyNameForGridView { get; set; }
        [DataMember]
        public string resourceNameForGridView { get; set; }
        [DataMember]
        public bool isMandatory { get; set; }
        [DataMember]
        public bool visibilityInGridView { get; set; }
        [DataMember]
        public bool visibilityInCreate { get; set; }
        [DataMember]
        public bool isEditableInCreate { get; set; }
        [DataMember]
        public bool visibilityInEdit { get; set; }
        [DataMember]
        public bool isEditableInEdit { get; set; }
        [DataMember]
        public string templateName { get; set; }
        [DataMember]
        public bool visibility { get; set; }
        [DataMember]
        public string validationRule { get; set; }
        [DataMember]
        public Nullable<int> sortOrder { get; set; }

        public TableProfileMetadataBools metadataBool { get; set; }
        public TableProfileMetadataDateTimes metadataDateTime { get; set; }
        public TableProfileMetadataDecimals metadataDecimal { get; set; }
        public TableProfileMetadataIntegers metadataInteger { get; set; }
        public TableProfileMetadataLists metadataList { get; set; }
        public TableProfileMetadataTexts metadataText { get; set; }

    }

    [DataContract]
    public class TableProfileMetadataDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }
        [DataMember]
        public List<TableProfileMetadatas> lstTableProfileMetadata { get; set; }
    }

    [DataContract]
    public class TableProfileMetadataBools
    {
        [DataMember]
        public int tableProfileMetadata_Id { get; set; }
        [DataMember]
        public string selectionOption { get; set; }
        [DataMember]
        public string defaultValue { get; set; }
    }

    [DataContract]
    public class TableProfileMetadataDateTimes
    {
        [DataMember]
        public int tableProfileMetadata_Id { get; set; }
        [DataMember]
        public Nullable<System.DateTime> minDate { get; set; }
        [DataMember]
        public Nullable<System.DateTime> maxDate { get; set; }
        [DataMember]
        public Nullable<bool> isAllowPastDate { get; set; }
        [DataMember]
        public Nullable<bool> isFollowCalendarSetting { get; set; }
        [DataMember]
        public Nullable<bool> isCurrentDateDefault { get; set; }
    }

    [DataContract]
    public class TableProfileMetadataDecimals
    {
        [DataMember]
        public int tableProfileMetadata_Id { get; set; }
        [DataMember]
        public Nullable<decimal> minValue { get; set; }
        [DataMember]
        public Nullable<decimal> maxValue { get; set; }
        [DataMember]
        public Nullable<decimal> defaultValue { get; set; }
    }

    [DataContract]
    public class TableProfileMetadataIntegers
    {
        [DataMember]
        public int tableProfileMetadata_Id { get; set; }
        [DataMember]
        public Nullable<int> minValue { get; set; }
        [DataMember]
        public Nullable<int> maxValue { get; set; }
        [DataMember]
        public Nullable<int> defaultValue { get; set; }
    }

    [DataContract]
    public class TableProfileMetadataLists
    {
        [DataMember]
        public int tableProfileMetadata_Id { get; set; }
        [DataMember]
        public bool isMultiValue { get; set; }
        [DataMember]
        public bool hasParent { get; set; }
        [DataMember]
        public string parentFieldName { get; set; }
        [DataMember]
        public Nullable<int> lookUpValue { get; set; }
        [DataMember]
        public Nullable<int> lookDefaultValue { get; set; }
        [DataMember]
        public string dataRetrieveURL { get; set; }


        [DataMember]
        public List<TableProfileMetadataListValues> lstListValue { get; set; }
    }

    [DataContract]
    public class TableProfileMetadataListValues
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int tableProfileMetadataList_Id { get; set; }
        [DataMember]
        public string fieldText { get; set; }
    }

    [DataContract]
    public class TableProfileMetadataTexts
    {
        [DataMember]
        public int tableProfileMetadata_Id { get; set; }
        [DataMember]
        public Nullable<int> minLength { get; set; }
        [DataMember]
        public Nullable<int> maxLength { get; set; }
        [DataMember]
        public string defaultValue { get; set; }
    }
}
