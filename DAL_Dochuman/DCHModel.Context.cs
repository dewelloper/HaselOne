﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DCHEntities : DbContext
    {
        public DCHEntities()
            : base("name=DCHEntities")
        {
           
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<HSL_CARI> HSL_CARI { get; set; }
        public virtual DbSet<HSL_LOKASYON> HSL_LOKASYON { get; set; }
        public virtual DbSet<HSL_YETKILI> HSL_YETKILI { get; set; }
        public virtual DbSet<HSV_CASABIT> HSV_CASABIT { get; set; }
        public virtual DbSet<DCH_SEKTOR> DCH_SEKTOR { get; set; }
        public virtual DbSet<DFSUserSet> DFSUserSet { get; set; }
        public virtual DbSet<DCH_KULLANICIBOLGE> DCH_KULLANICIBOLGE { get; set; }
        public virtual DbSet<DCV_KULLANICIBOLGE> DCV_KULLANICIBOLGE { get; set; }
    }
}