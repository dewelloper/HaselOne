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
    
    public partial class Cm_CustomerInterviews
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public Nullable<int> AuthenticatorId { get; set; }
        public int InterviewTypeId { get; set; }
        public Nullable<System.DateTime> InterviewDate { get; set; }
        public string Note { get; set; }
        public bool Interviewed { get; set; }
        public Nullable<int> ImportantId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreateUserId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> UpdateUserId { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> DeleteUserId { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
    
        public virtual Cm_CustomerAuthenticators Cm_CustomerAuthenticators { get; set; }
        public virtual Cm_Interview Cm_Interview { get; set; }
        public virtual Gn_InterviewImportant Gn_InterviewImportant { get; set; }
        public virtual Gn_User Gn_User { get; set; }
        public virtual Gn_User Gn_User1 { get; set; }
        public virtual Gn_User Gn_User2 { get; set; }
        public virtual Gn_User Gn_User3 { get; set; }
    }
}