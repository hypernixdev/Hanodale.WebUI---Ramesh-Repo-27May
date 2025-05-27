using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class TableProfiles
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public Nullable<int> menu_Id { get; set; }
        [DataMember]
        public Nullable<int> parentMenu_Id { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public int localizationResource_Id { get; set; }
        [DataMember]
        public string icon { get; set; }
        [DataMember]
        public int sortOrder { get; set; }
        [DataMember]
        public bool visibility { get; set; }
        [DataMember]
        public string resourceNameKey { get; set; }
        

        [DataMember]
        public List<TableProfiles> lstTableProfileTabs { get; set; }

        //[DataMember]
        //public List<TableProfileMetadatas> lstTableProfileMetadatas { get; set; }
    }
    public class TableProfileDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }
        [DataMember]
        public List<TableProfiles> lstTableProfile { get; set; }
    }
}
