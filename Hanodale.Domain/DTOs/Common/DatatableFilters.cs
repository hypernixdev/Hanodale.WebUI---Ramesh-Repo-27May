using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class DatatableFilters
    {
        [DataMember]
        public int currentUserId { get; set; }

        [DataMember]
        public int masterRecord_Id { get; set; }

        [DataMember]
        public bool all { get; set; }
        
        [DataMember]
        public int startIndex { get; set; }
        
        [DataMember]
        public int pageSize { get; set; }
        
        [DataMember]
        public string search { get; set; }

        [DataMember]
        public string conditionType { get; set; }

        [DataMember]
        public string OrderDateFrom { get; set; }

        [DataMember]
        public string OrderDateTo { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string OrderNum { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }
    }
}
