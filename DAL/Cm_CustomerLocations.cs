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
    
    public partial class Cm_CustomerLocations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cm_CustomerLocations()
        {
            this.Cm_CustomerMachineparks = new HashSet<Cm_CustomerMachineparks>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public Nullable<bool> IsFat { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public Nullable<int> CreatorId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> ModifierId { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> DeleterId { get; set; }
        public Nullable<System.DateTime> DeleterDate { get; set; }
    
        public virtual Cm_Customer Cm_Customer { get; set; }
        public virtual Gn_User Gn_User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cm_CustomerMachineparks> Cm_CustomerMachineparks { get; set; }
    }
}