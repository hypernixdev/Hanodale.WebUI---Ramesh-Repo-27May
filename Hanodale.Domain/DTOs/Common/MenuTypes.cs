using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class MenuTypes
    {
        [DataMember]
        public bool isWorkOrder { get; set; }
        [DataMember]
        public bool isPurchaseRequest { get; set; }
        [DataMember]
        public bool isPurchaseOrder { get; set; }
        [DataMember]
        public bool isRFQ { get; set; }
        [DataMember]
        public bool isHelpDesk { get; set; }
        [DataMember]
        public bool hasModuleItem { get; set; }
        [DataMember]
        public bool isBusiness { get; set; }
        [DataMember]
        public Dictionary<string, string> appSettingList = new Dictionary<string, string>();
    }
}
