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
    
    public partial class UserProfile
    {
        public int id { get; set; }
        public string userName { get; set; }
        public int userType_Id { get; set; }
        public int person_Id { get; set; }
        public string password { get; set; }
        public string passwordHash { get; set; }
        public Nullable<bool> verified { get; set; }
        public Nullable<int> language { get; set; }
        public bool isActive { get; set; }
        public string createdBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }
    
        public virtual UserType UserType { get; set; }
    }
}
