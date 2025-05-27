using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class ChartWOInfo
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int organizationId { get; set; }

        [DataMember]
        public string filterType { get; set; }

        [DataMember]
        public List<string> typeList { get; set; }

        [DataMember]
        public DateTime loadDateFrom { get; set; }

        [DataMember]
        public DateTime loadDateTo { get; set; }

        [DataMember]
        public string workOrderSectionType { get; set; }

        [DataMember]
        public string module { get; set; }
    }
}
