﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LOIS.DAL.Lois
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class loisEntities1 : DbContext
    {
        public loisEntities1()
            : base("name=loisEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Authentication> Authentications { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<RequisitionDocument> RequisitionDocuments { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<Requisition> Requisitions { get; set; }
        public virtual DbSet<AuthenticationGroup> AuthenticationGroups { get; set; }
        public virtual DbSet<AuthGroupUser> AuthGroupUsers { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}