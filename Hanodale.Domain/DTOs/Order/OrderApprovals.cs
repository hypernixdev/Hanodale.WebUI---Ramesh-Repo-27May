using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class OrderApprovals
    {
        [DataMember]
        
        public int id { get; set; }
        [DataMember]
        public int order_Id { get; set; }

        [DataMember]
        public string orderNum { get; set; }
        [DataMember]
        public int submittedUser_Id { get; set; }
        [DataMember]
        public System.DateTime submittedDate { get; set; }
        [DataMember]
        public string approvalStatus { get; set; }
        [DataMember]
        public string remarks { get; set; }
        [DataMember]
        public Nullable<int> approvedUser_Id { get; set; }
        [DataMember]
        public Nullable<System.DateTime> approvedDate { get; set; }
        [DataMember]
        public Nullable<System.DateTime> OrderDate { get; set; }
        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string OrderStatus { get; set; }

        [DataMember]
        public decimal OrderTotal { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public string ApprovalBy { get; set; }

        [DataMember]
        public string Remarks { get; set; }

        [DataMember]
        public bool isSuccess { get; set; }

        [DataMember]
        public int customerid { get; set; }




    }
    public class OrderApprovalDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<OrderApprovals> lstOrderApproval { get; set; }
    }


}
