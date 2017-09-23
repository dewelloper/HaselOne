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
    
    public partial class Cm_MachineparkRental
    {
        public int Id { get; set; }
        public int MachineparkId { get; set; }
        public int CustomerId { get; set; }
        public Nullable<int> RequestId { get; set; }
        public System.DateTime BeginDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public int CreateUserId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> UpdateUserId { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> DeleteUserId { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public bool IsAborted { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Cm_Customer Cm_Customer { get; set; }
        public virtual Cm_CustomerMachineparks Cm_CustomerMachineparks { get; set; }
        public virtual Cm_CustomerRequest Cm_CustomerRequest { get; set; }
        public virtual Gn_User Gn_User { get; set; }
        public virtual Gn_User Gn_User1 { get; set; }
        public virtual Gn_User Gn_User2 { get; set; }
    }
}
