using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class ChartInfo
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
        public List<ChartInfoSubTypeList> typeGroupList { get; set; }

        [DataMember]
        public DateTime loadDateFrom { get; set; }

        [DataMember]
        public DateTime loadDateTo { get; set; }

        [DataMember]
        public string sectionType { get; set; }

        [DataMember]
        public string sortColumn { get; set; }

        [DataMember]
        public string sortType { get; set; }

        [DataMember]
        public string module { get; set; }
    }

    public class ChartInfoSubTypeList
    {
        [DataMember]
        public string section { get; set; }

        [DataMember]
        public List<string> typeList { get; set; }

    }
}
