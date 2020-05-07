using System;
using System.Collections.Generic;

namespace LOIS.Core.Models
{
    public class Requisition
    {
        public int RequisitionNo { get; set; }
        public string EmrNo { get; set; }
        public DateTime? AccessionDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public int? SpecimineType { get; set; }
        public int? PatientId { get; set; }
        public int? PrimePayerId { get; set; }
        public int? SecondaryPayerId { get; set; }
        public Nullable<int> ProviderId { get; set; }
        public string ProviderName { get; set; }
        public Nullable<DateTime> CollectedDate { get; set; }
        public DateTime? ScannedDate { get; set; }

        public Patient Patient { get; set; }
        public List<RequisitionDocument> Documents { get; set; }
    }
}
