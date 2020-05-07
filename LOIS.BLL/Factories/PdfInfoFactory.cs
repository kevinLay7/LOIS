using LOIS.Core.Models;
using System.IO;

namespace LOIS.BLL.Factories
{
    public class PdfInfoFactory
    {
        public static PDFInfo CreatePdfInfo(string filepath, DTO.Rectangle rectangle)
        {
            var pdf = new PDFInfo();
            pdf.FilePath = filepath;
            pdf.CreatedTime = new FileInfo(filepath).CreationTime;
            pdf.Images = new Imaging.PdfConverter().CovertPdfToImages(filepath);

            foreach(var image in pdf.Images)
            {
                var bcode = Imaging.BarcodeScanner.GetBarcode(image, rectangle);
                
                if (bcode.Type != Core.Enums.BarcodeType.Empty)
                {
                    pdf.Barcode = bcode.Number;
                    pdf.BarcodeType = bcode.Type;
                    pdf.isBarcode = true;
                    break;
                }
            }

            //Loop over the images again to compress them.  Can't modify the itterator in the foreach loop
            for(int i = 0; i < pdf.Images.Count; i++)
            {
                var img = pdf.Images[i];
                //image has been processed.  We can compress it now
                Imaging.ImageCompressor.CompressImage(ref img);
            }
                       
            return pdf;
        }

    }
}
