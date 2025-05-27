using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class BusinessOtherss
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int business_Id { get; set; }
        [DataMember]
        public Nullable<decimal> bumiShare { get; set; }
        [DataMember]
        public Nullable<decimal> nonBumiShare { get; set; }
        [DataMember]
        public Nullable<decimal> foreignShare { get; set; }
        [DataMember]
        public Nullable<decimal> bumiCapital { get; set; }
        [DataMember]
        public Nullable<bool> classA { get; set; }
        [DataMember]
        public Nullable<bool> classB { get; set; }
        [DataMember]
        public Nullable<bool> classBX { get; set; }
        [DataMember]
        public Nullable<bool> classC { get; set; }
        [DataMember]
        public Nullable<bool> classD { get; set; }
        [DataMember]
        public Nullable<bool> classE { get; set; }
        [DataMember]
        public Nullable<bool> classEX { get; set; }
        [DataMember]
        public Nullable<bool> classF { get; set; }
        [DataMember]
        public Nullable<bool> pkk { get; set; }
        [DataMember]
        public Nullable<bool> tnb { get; set; }
        [DataMember]
        public Nullable<bool> jba { get; set; }
        [DataMember]
        public Nullable<bool> mara { get; set; }
        [DataMember]
        public Nullable<bool> dbkl { get; set; }
        [DataMember]
        public Nullable<bool> financeMinistry { get; set; }
        [DataMember]
        public Nullable<bool> jkh { get; set; }
        [DataMember]
        public Nullable<bool> jkr { get; set; }
        [DataMember]
        public string createdBy { get; set; }
        [DataMember]
        public System.DateTime createdDate { get; set; }
        [DataMember]
        public string modifiedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }
        [DataMember]
        public string businessCategory { get; set; }
        [DataMember]
        public Nullable<int> paidUpCapital { get; set; }
    }

    public class BusinessOthersDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<BusinessOtherss> lstBusinessOther { get; set; }
    }
}
