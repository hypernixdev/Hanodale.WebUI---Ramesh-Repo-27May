using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class OrderUpdates
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public Nullable<int> user_Id { get; set; }
        [DataMember]
        public string actionName { get; set; }
        [DataMember]
        public string userName { get; set; }
        [DataMember]
        public Nullable<System.DateTime> actionDate { get; set; }
        [DataMember]

        public string actionDateFormatted { get; set; } // New property for formatted date

        [DataMember]
        public Nullable<int> order_Id { get; set; }
        [DataMember]
        public string orderStatus { get; set; }

    }

  
}
