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
    
    public partial class Gn_Notifications
    {
        public int Id { get; set; }
        public Nullable<int> ModulId { get; set; }
        public Nullable<int> PageId { get; set; }
        public Nullable<int> CrudId { get; set; }
        public Nullable<int> SenderUserId { get; set; }
        public int TargetUserId { get; set; }
        public System.DateTime SendDate { get; set; }
        public Nullable<System.DateTime> ReceiveDate { get; set; }
        public string NotifyMessage { get; set; }
        public int CreatorId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> ModifierId { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> DeleterId { get; set; }
        public Nullable<System.DateTime> DeleteDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual Gn_ModulsAndMenus Gn_ModulsAndMenus { get; set; }
        public virtual Gn_User Gn_User { get; set; }
        public virtual Gn_User Gn_User1 { get; set; }
    }
}
