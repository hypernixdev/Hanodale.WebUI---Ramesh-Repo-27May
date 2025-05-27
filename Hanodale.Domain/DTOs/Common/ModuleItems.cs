using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class ModuleItems
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int modulType_Id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public bool visibility { get; set; }
        [DataMember]
        public string remarks { get; set; }
        [DataMember]
        public int sortOrder { get; set; }
        [DataMember]
        public string moduleName { get; set; }
    }

    public class ModuleItemDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<ModuleItems> lstModuleItem { get; set; }
    }
}
