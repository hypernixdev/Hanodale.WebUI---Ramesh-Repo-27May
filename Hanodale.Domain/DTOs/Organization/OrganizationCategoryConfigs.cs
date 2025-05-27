using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hanodale.Domain.DTOs
{
    [DataContract]
    public class OrganizationCategoryConfigs
    {
        [DataMember]
        public int category_Id { get; set; }

        [DataMember]
        public string categoryName { get; set; }

        [DataMember]
        public int childCategory_Id { get; set; }

        [DataMember]
        public string childCategoryName { get; set; }


        [DataMember]
        public string categoryIcon { get; set; }
    }
}
