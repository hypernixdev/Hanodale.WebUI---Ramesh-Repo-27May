using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class Menu
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string menuName { get; set; }

        [DataMember]
        public string pageName { get; set; }

        [DataMember]
        public string pageUrl { get; set; }

        [DataMember]
        public int? ordering { get; set; }

        [DataMember]
        public string imageUrl { get; set; }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public List<SubMenus> subMenus { get; set; }

    }

    [DataContract]
    public class SubMenus
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int mainMenu_Id { get; set; }

        [DataMember]
        public string subMenuName { get; set; }

        [DataMember]
        public string pageName { get; set; }

        [DataMember]
        public string pageUrl { get; set; }

        [DataMember]
        public string imageUrl { get; set; }

        [DataMember]
        public Nullable<bool> isMainMenu { get; set; }

        [DataMember]
        public Nullable<int> reportCategory_Id { get; set; }

        [DataMember]
        public UserRights userRights { get; set; }

        [DataMember]
        public int? ordering { get; set; }
    }
}