using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    public class HelpDesks
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int organization_Id { get; set; }
        [DataMember]
        public Nullable<int> user_Id { get; set; }
        [DataMember]
        public Nullable<int> asset_Id { get; set; }
        [DataMember]
        public int workFollowStatus_Id { get; set; }
        [DataMember]
        public string code { get; set; }
        [DataMember]
        public string prefix { get; set; }
        [DataMember]
        public string feedback { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string department { get; set; }
        [DataMember]
        public string designation { get; set; }
        [DataMember]
        public string officePhone { get; set; }
        [DataMember]
        public string cellPhone { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string receivedBy { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public DateTime createdDate { get; set; }
        [DataMember]
        public DateTime? receivedDate { get; set; }
        [DataMember]
        public string modifiedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }
        [DataMember]
        public string workFollowStatusName { get; set; }
        [DataMember]
        public string remarks { get; set; }
        [DataMember]
        public bool newFlag { get; set; }
        [DataMember]
        public bool receiveFlag { get; set; }
        [DataMember]
        public bool actionFlag { get; set; }

        [DataMember]
        public DateTime? inprogressDate { get; set; }
        [DataMember]
        public DateTime? actionDate { get; set; }

        public System.Collections.Generic.IEnumerable<String> woCodes { get; set; }

        [DataMember]
        public List<string> fileNames { get; set; }

        [DataMember]
        public List<string> removeFileName { get; set; }

        [DataMember]
        public bool isPass { get; set; }

        [DataMember]
        public bool isCreate { get; set; }

        [DataMember]
        public bool isReceived { get; set; }

        [DataMember]
        public DateTime? createdDateFrom { get; set; }

        [DataMember]
        public DateTime? createdDateTo { get; set; }

        [DataMember]
        public string feedbackModule { get; set; }
    }
    public class HelpDeskDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<HelpDesks> lstHelpDesk { get; set; }
    }
}
