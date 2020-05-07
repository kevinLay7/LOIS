using System;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PDFiumSharp;
using System.Windows.Media.Imaging;
using MoreLinq;

namespace LOIS.BLL.Imaging
{
    public class PdfConverter
    {
        //Todo create one public method that will choose which method to use.  Try Itext, and if fails fall back to pdfium

        public List<byte[]> CovertPdfToImages(string filepath)
        {
            List<byte[]> images = new List<byte[]>();

            using (var reader = new PdfReader(filepath))
            {
                var parser = new PdfReaderContentParser(reader);
                ImageRenderListener listener = null;

                for (var i = 1; i <= reader.NumberOfPages; i++)
                {
                    parser.ProcessContent(i, (listener = new ImageRenderListener()));

                    if (listener.Images.Count > 0)
                    {
                        var imgs = listener.Images.Select(pair => pair.Key);

                        foreach(var img in imgs)
                        {
                            var data = img.Encode(SKEncodedImageFormat.Jpeg, 100);
                            var ms = new MemoryStream();
                            data.SaveTo(ms);
                            images.Add(ms.ToArray());
                            ms.Dispose();
                        }
                    }
                }
            }
            return images;
        }

        //Todo clean this up and add support for multiple pages
        public byte[] Test(string filepath)
        {
            using (var doc = new PDFiumSharp.PdfDocument(filepath))
            {
                var page = doc.Pages[0];
                using (var bitmap = new PDFiumBitmap((int)page.Width, (int)page.Height, false))
                {
                    page.Render(bitmap);
                    var b = new BmpBitmapDecoder(bitmap.AsBmpStream(), BitmapCreateOptions.None, BitmapCacheOption.OnLoad).Frames.First();
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(b);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        return ms.ToArray();
                    }
                }
            }
        }

    }

    //Class taken from https://psycodedeveloper.wordpress.com/2013/01/10/how-to-extract-images-from-pdf-files-using-c-and-itextsharp/
    internal class ImageRenderListener : IRenderListener
    {
        #region Fields
        Dictionary<SKImage, string> images = new Dictionary<SKImage, string>();
        #endregion Fields
        #region Properties
        public Dictionary<SKImage, string> Images
        {
            get { return images; }
        }
        #endregion Properties
        #region Methods
        #region Public Methods
        public void BeginTextBlock() { }
        public void EndTextBlock() { }
        public void RenderImage(ImageRenderInfo renderInfo)
        {
            PdfImageObject image = renderInfo.GetImage();
            PdfName filter = (PdfName)image.Get(PdfName.FILTER);

           
            if (filter != null)
            {
                var drawingImage = image.GetImageAsBytes();
                string extension = ".";
                if (filter == PdfName.DCTDECODE)
                {
                    extension += PdfImageObject.ImageBytesType.JPG.FileExtension;
                }
                else if (filter == PdfName.JPXDECODE)
                {
                    extension += PdfImageObject.ImageBytesType.JP2.FileExtension;
                }
                else if (filter == PdfName.FLATEDECODE)
                {
                    extension += PdfImageObject.ImageBytesType.PNG.FileExtension;
                }
                else if (filter == PdfName.LZWDECODE)
                {
                    extension += PdfImageObject.ImageBytesType.CCITT.FileExtension;
                }
                /* Rather than struggle with the image stream and try to figure out how to handle
                    * BitMapData scan lines in various formats (like virtually every sample I’ve found
                    * online), use the PdfImageObject.GetDrawingImage() method, which does the work for us. */

                var skbit = SkiaSharp.SKBitmap.Decode(drawingImage);
                var skimg = SKImage.FromBitmap(skbit);
                this.Images.Add(skimg, extension);
            }
        }
        public void RenderText(TextRenderInfo renderInfo) { }
        #endregion Public Methods
        #endregion Methods
    }
}
