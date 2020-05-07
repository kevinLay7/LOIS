using LOIS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOIS.Core.Models
{
    public class Barcode
    {
        public string Number { get; set; }
        public BarcodeType Type { get; set; }
    }
}
