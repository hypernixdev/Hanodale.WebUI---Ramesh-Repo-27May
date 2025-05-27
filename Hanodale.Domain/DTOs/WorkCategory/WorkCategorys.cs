using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class WorkCategorys
    {
        [DataMember]
        public int id { get; set; } 

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string remarks { get; set; }

        [DataMember]
        public bool isVisible { get; set; }

        [DataMember]
        public string createdBy { get; set; }

        [DataMember]
        public System.DateTime createdDate { get; set; }

        [DataMember]
        public string modifiedBy { get; set; }

        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }
       
        [DataMember]
        public bool isSelected { get; set; }

    }

    public class WorkCategoryDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<WorkCategorys> lstWorkCategory { get; set; }
    }
}





