using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class MainMenus
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public Nullable<int> reference_Id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string pageName { get; set; }
        [DataMember]
        public string pageUrl { get; set; }
        [DataMember]
        public string imageUrl { get; set; }
        [DataMember]
        public Nullable<int> ordering { get; set; }
        [DataMember]
        public Nullable<int> menuType_Id { get; set; }
        [DataMember]
        public Nullable<int> menuLevel_Id { get; set; }
        [DataMember]
        public Nullable<int> reportCategory_Id { get; set; }
        [DataMember]
        public bool visibility { get; set; }
        [DataMember]
        public bool isActive { get; set; }
        [DataMember]
        public bool showAsMain { get; set; }
    }

    public class MainMenuDetails
    {
        [DataMember]
        public RecordDetails recordDetails { get; set; }

        [DataMember]
        public List<MainMenus> lstMainMenu { get; set; }
    }
}
