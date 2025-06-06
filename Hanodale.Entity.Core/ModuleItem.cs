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
    
    public partial class ModuleItem
    {
        public ModuleItem()
        {
            this.AdhocReports = new HashSet<AdhocReport>();
            this.Budgets = new HashSet<Budget>();
            this.Businesses = new HashSet<Business>();
            this.Businesses1 = new HashSet<Business>();
            this.BusinessClassifications = new HashSet<BusinessClassification>();
            this.BusinessFiles = new HashSet<BusinessFile>();
            this.CompanyProfiles = new HashSet<CompanyProfile>();
            this.CompanyProfiles1 = new HashSet<CompanyProfile>();
            this.FileUploadHistories = new HashSet<FileUploadHistory>();
            this.HelpDesks = new HashSet<HelpDesk>();
            this.Language_Profile = new HashSet<Language_Profile>();
            this.OrganizationEmails = new HashSet<OrganizationEmail>();
            this.Leave_Carry_Forward = new HashSet<Leave_Carry_Forward>();
            this.Leave_Carry_Forward1 = new HashSet<Leave_Carry_Forward>();
            this.Leave_Carry_Forward2 = new HashSet<Leave_Carry_Forward>();
            this.Leave_Sequence = new HashSet<Leave_Sequence>();
            this.Leave_Sequence1 = new HashSet<Leave_Sequence>();
            this.Leave_View_Apply = new HashSet<Leave_View_Apply>();
            this.Leave_View_Apply1 = new HashSet<Leave_View_Apply>();
            this.Leave_View_Apply2 = new HashSet<Leave_View_Apply>();
            this.Leave_View_Apply3 = new HashSet<Leave_View_Apply>();
            this.Users = new HashSet<User>();
            this.OrderItemDeleted = new HashSet<OrderItemDeleted>();
            this.OrderItemDeleted1 = new HashSet<OrderItemDeleted>();
            this.OrderItemDeleted2 = new HashSet<OrderItemDeleted>();
            this.OrderItems = new HashSet<OrderItems>();
            this.OrderItems1 = new HashSet<OrderItems>();
            this.OrderItems2 = new HashSet<OrderItems>();
        }
    
        public int id { get; set; }
        public int modulType_Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool visibility { get; set; }
        public int sortOrder { get; set; }
        public string remarks { get; set; }
    
        public virtual ICollection<AdhocReport> AdhocReports { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Business> Businesses { get; set; }
        public virtual ICollection<Business> Businesses1 { get; set; }
        public virtual ICollection<BusinessClassification> BusinessClassifications { get; set; }
        public virtual ICollection<BusinessFile> BusinessFiles { get; set; }
        public virtual ICollection<CompanyProfile> CompanyProfiles { get; set; }
        public virtual ICollection<CompanyProfile> CompanyProfiles1 { get; set; }
        public virtual ICollection<FileUploadHistory> FileUploadHistories { get; set; }
        public virtual ICollection<HelpDesk> HelpDesks { get; set; }
        public virtual ICollection<Language_Profile> Language_Profile { get; set; }
        public virtual ModuleType ModuleType { get; set; }
        public virtual ICollection<OrganizationEmail> OrganizationEmails { get; set; }
        public virtual ICollection<Leave_Carry_Forward> Leave_Carry_Forward { get; set; }
        public virtual ICollection<Leave_Carry_Forward> Leave_Carry_Forward1 { get; set; }
        public virtual ICollection<Leave_Carry_Forward> Leave_Carry_Forward2 { get; set; }
        public virtual ICollection<Leave_Sequence> Leave_Sequence { get; set; }
        public virtual ICollection<Leave_Sequence> Leave_Sequence1 { get; set; }
        public virtual ICollection<Leave_View_Apply> Leave_View_Apply { get; set; }
        public virtual ICollection<Leave_View_Apply> Leave_View_Apply1 { get; set; }
        public virtual ICollection<Leave_View_Apply> Leave_View_Apply2 { get; set; }
        public virtual ICollection<Leave_View_Apply> Leave_View_Apply3 { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<OrderItemDeleted> OrderItemDeleted { get; set; }
        public virtual ICollection<OrderItemDeleted> OrderItemDeleted1 { get; set; }
        public virtual ICollection<OrderItemDeleted> OrderItemDeleted2 { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual ICollection<OrderItems> OrderItems1 { get; set; }
        public virtual ICollection<OrderItems> OrderItems2 { get; set; }
    }
}
