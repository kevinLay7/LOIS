//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Requisition
    {
        public int RequisitionNo { get; set; }
        public string EmrNo { get; set; }
        public Nullable<System.DateTime> AccessionDate { get; set; }
        public Nullable<int> SpecimineType { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public Nullable<int> PatientId { get; set; }
        public Nullable<int> PrimePayerId { get; set; }
        public Nullable<int> SecondaryPayerId { get; set; }
        public Nullable<int> ProviderId { get; set; }
        public string ProviderName { get; set; }
        public Nullable<System.DateTime> CollectedDate { get; set; }
        public Nullable<System.DateTime> ScannedDate { get; set; }
    }
}
