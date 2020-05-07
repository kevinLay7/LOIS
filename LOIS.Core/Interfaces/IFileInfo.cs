using LOIS.Core.Enums;
using LOIS.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOIS.Core.Interfaces
{
    interface IFileInfo
    {
        bool BarcodPage { get; set; }
        string Barcode { get; set; }
        BarcodeType BarcodeType { get; set; }
        string FilePath { get; set; }
        RequisitionType RequisitionType { get; set; }
        DateTime CreatedTime { get; set; }
        MemoryStream PdfMemoryStream { get; set; }
    }
}
