using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOIS.Core.Models
{
    public class User
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public Nullable<bool> enabled { get; set; }
        public Nullable<bool> admin { get; set; }
        public string passwordhash { get; set; }
        public string salt { get; set; }

        public IEnumerable<string> Groups { get; set; }
    }
}
