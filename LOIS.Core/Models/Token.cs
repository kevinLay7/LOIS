using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOIS.Core.Models
{
    public class Token
    {
        public int tokenid { get; set; }
        public int userid { get; set; }
        public string authtoken { get; set; }
        public Nullable<System.DateTime> issuedon { get; set; }
        public Nullable<System.DateTime> expireson { get; set; }
    }
}
