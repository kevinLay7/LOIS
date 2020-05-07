using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOIS.Core.Models
{
    public class RequisitionSearch
    {
        public int? RequisitionId { get; set; }
        public string EMRNo { get; set; }
        public DateTime? AccessionDateStart { get; set; }
        public DateTime? AccessionDateEnd { get; set; }
        public DateTime? CollectedDateStart { get; set; }
        public DateTime? CollectedDateEnd { get; set; }
        public int? PatientId { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public int? PatientSSN { get; set; }
        public string PrimaryPayer { get; set; }
        public DateTime? ScannedDateStart { get; set; }
        public DateTime? ScannedDateEnd { get; set; }
    }
}
