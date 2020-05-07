using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using LOIS.BLL.Imaging;

namespace LOIS.BLL.Documents
{
    public class PdfCreator : IDisposable
    {
        private Document document;
        private MemoryStream stream;
        private PdfWriter writer;

        public PdfCreator()
        {
            document = new Document();
            document.SetMargins(0f, 0f, 0f, 0f);

            stream = new MemoryStream();
            writer = PdfWriter.GetInstance(document, stream);
            writer.CloseStream = false;
            document.Open();
        }

        public void AppendImageToPdf(byte[] image, bool compressFirst = true)
        {
            if(compressFirst)
                ImageCompressor.CompressImage(ref image);

            var img = iTextSharp.text.Image.GetInstance(image);
            img.ScalePercent(24f);
            
            document.NewPage();
            document.Add(img);
        }

        public byte[] GetPdfDocumentBytes()
        {
            document.Close();
            writer.Close();
            return stream.ToArray();
        }

        public void Dispose()
        {
            try
            {
                writer.Dispose();
                document.Dispose();
            }
            catch (Exception)
            {

            }
        }
    }
}
