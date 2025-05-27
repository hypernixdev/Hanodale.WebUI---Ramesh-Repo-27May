using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class UserRights
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int userRole_Id { get; set; }

        [DataMember]
        public int menuItem_Id { get; set; }

        [DataMember]
        public bool canView { get; set; }

        [DataMember]
        public bool canAdd { get; set; }

        [DataMember]
        public bool canEdit { get; set; }

        [DataMember]
        public bool canDelete { get; set; }

        [DataMember]
        public string createdBy { get; set; }

        [DataMember]
        public System.DateTime createdDate { get; set; }

        [DataMember]
        public string modifiedBy { get; set; }

        [DataMember]
        public Nullable<System.DateTime> modifiedDate { get; set; }

        [DataMember]
        public SubMenus subMenus { get; set; }

    }
}