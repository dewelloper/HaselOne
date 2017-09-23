//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cm_CustomerRequest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cm_CustomerRequest()
        {
            this.Cm_CustomerMachineparks = new HashSet<Cm_CustomerMachineparks>();
            this.Cm_MachineparkRental = new HashSet<Cm_MachineparkRental>();
        }
    
        public int Id { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> MarkId { get; set; }
        public Nullable<int> ModelId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public int SalesType { get; set; }
        public Nullable<System.DateTime> EstimatedBuyDate { get; set; }
        public Nullable<int> UseDuration { get; set; }
        public Nullable<int> UseDurationUnit { get; set; }
        public Nullable<int> OwnerId { get; set; }
        public int ChannelId { get; set; }
        public Nullable<int> SalesmanId { get; set; }
        public Nullable<short> ConditionType { get; set; }
        public Nullable<int> MonthlyWorkingHours { get; set; }
        public int ResultType { get; set; }
        public bool IsDeleted { get; set; }
        public int CreateUserId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> UpdateUserId { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> DeleteUserId { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public int CustomerId { get; set; }
        public System.DateTime RequestDate { get; set; }
        public string Note { get; set; }
    
        public virtual Cm_Customer Cm_Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cm_CustomerMachineparks> Cm_CustomerMachineparks { get; set; }
        public virtual Cm_MachineparkCategory Cm_MachineparkCategory { get; set; }
        public virtual Cm_MachineparkMark Cm_MachineparkMark { get; set; }
        public virtual Gn_ConnectionChannel Gn_ConnectionChannel { get; set; }
        public virtual Gn_User Gn_User { get; set; }
        public virtual Gn_User Gn_User1 { get; set; }
        public virtual Gn_User Gn_User2 { get; set; }
        public virtual Gn_User Gn_User3 { get; set; }
        public virtual Gn_User Gn_User4 { get; set; }
        public virtual Pr_MachineModel Pr_MachineModel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cm_MachineparkRental> Cm_MachineparkRental { get; set; }
    }
}
