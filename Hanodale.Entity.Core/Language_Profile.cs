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
    
    public partial class Language_Profile
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public Nullable<int> ModuleItem_Byte_Id { get; set; }
    
        public virtual ModuleItem ModuleItem { get; set; }
    }
}
