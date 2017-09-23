//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL_Dochuman
{
    using System;
    using System.Collections.Generic;
    
    public partial class DFSUserSet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DFSUserSet()
        {
            this.DFSUserSet1 = new HashSet<DFSUserSet>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string WindowsName { get; set; }
        public byte Locked { get; set; }
        public Nullable<System.DateTime> LastLogonTime { get; set; }
        public Nullable<System.DateTime> LastSyncTime { get; set; }
        public Nullable<int> ManagerId { get; set; }
        public string Title { get; set; }
        public string EMailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<int> DFSGroupId { get; set; }
        public Nullable<byte> DesignTimeAdmin { get; set; }
        public Nullable<byte> RunTimeAdmin { get; set; }
        public string CustomInfo { get; set; }
        public Nullable<int> CustomKey { get; set; }
        public Nullable<byte> FormUser { get; set; }
        public Nullable<byte> DocMngUser { get; set; }
        public Nullable<int> OneId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DFSUserSet> DFSUserSet1 { get; set; }
        public virtual DFSUserSet DFSUserSet2 { get; set; }
    }
}