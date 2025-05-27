using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class ModuleCodes
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public Nullable<int> menu_Id { get; set; }
        [DataMember]
        public string prefix { get; set; }
        [DataMember]
        public string mask { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> createdDate { get; set; }
        [DataMember]
        public string modifiedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }
        [DataMember]
        public string generateCode { get; set; }
    }
    public class ModuleCodeDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<ModuleCodes> lstModuleCode { get; set; }
    }
}
