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
    
    public partial class Gn_DepartmentRoles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Gn_DepartmentRoles()
        {
            this.Gn_UserRoles = new HashSet<Gn_UserRoles>();
        }
    
        public int Id { get; set; }
        public Nullable<int> GroupId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> RuleId { get; set; }
    
        public virtual Gn_Category Gn_Category { get; set; }
        public virtual Gn_Department Gn_Department { get; set; }
        public virtual Gn_Role Gn_Role { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Gn_UserRoles> Gn_UserRoles { get; set; }
    }
}