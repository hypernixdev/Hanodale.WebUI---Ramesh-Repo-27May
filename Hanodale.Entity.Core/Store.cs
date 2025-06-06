//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hanodale.Entity.Core
{
    using System;
    using System.Collections.Generic;
    
    public partial class Store
    {
        public Store()
        {
            this.ShipToAddress = new HashSet<ShipToAddress>();
        }
    
        public int id { get; set; }
        public string company { get; set; }
        public string plant { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public Nullable<int> address_City_id { get; set; }
        public Nullable<int> address_State_id { get; set; }
        public Nullable<int> address_Country_id { get; set; }
        public string zip { get; set; }
    
        public virtual Address_City Address_City { get; set; }
        public virtual Address_Country Address_Country { get; set; }
        public virtual Address_State Address_State { get; set; }
        public virtual ICollection<ShipToAddress> ShipToAddress { get; set; }
    }
}
