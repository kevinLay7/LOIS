using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOIS.Core.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> SSN { get; set; }
        public string Address { get; set; }
        public Nullable<System.TimeSpan> DateOfBirth { get; set; }
    }
}
