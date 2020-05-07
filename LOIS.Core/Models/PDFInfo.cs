using LOIS.Core.Interfaces;
using System;
using LOIS.Core.Enums;
using System.IO;
using System.Collections.Generic;

namespace LOIS.Core.Models
{
    public class PDFInfo : IDisposable
    {
        public bool isBarcode { get; set; }
        public string Barcode { get; set; }
        public BarcodeType BarcodeType { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedTime { get; set; }
        public List<byte[]> Images { get; set; }
        public byte[] Pdf { get; set; }

        public void Dispose()
        {
            if(Images != null && Images.Count > 0)
            {
                foreach (var img in Images)
                    Array.Clear(img, 0, img.Length);
            }

            if(Pdf != null)
            {
                Array.Clear(Pdf, 0, Pdf.Length);
            }
        }
    }
}
