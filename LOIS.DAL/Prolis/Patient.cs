//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LOIS.DAL.Prolis
{
    using System;
    using System.Collections.Generic;
    
    public partial class Patient
    {
        public long ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Sex { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Ethnicity { get; set; }
        public Nullable<int> Race_ID { get; set; }
        public string SSN { get; set; }
        public Nullable<bool> IsAlive { get; set; }
        public Nullable<System.DateTime> DeathDate { get; set; }
        public Nullable<long> Address_ID { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SecretQ { get; set; }
        public string SecretA { get; set; }
        public string Fax { get; set; }
        public string Cell { get; set; }
        public Nullable<long> Employer_ID { get; set; }
        public string Note { get; set; }
    }
}
