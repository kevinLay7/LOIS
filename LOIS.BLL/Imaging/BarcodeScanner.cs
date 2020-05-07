using System;
using System.IO;
using SkiaSharp;
using ZXing;
using LOIS.Core.Models;
using LOIS.Core.Enums;

namespace LOIS.BLL.Imaging
{
    public static class BarcodeScanner
    {
        public static Barcode GetBarcode(byte[] imageBytes, DTO.Rectangle rectangle)
        {
            try
            {
                var barcode = GetCodeFromImage(imageBytes, rectangle);
                
                if(barcode == null)
                    barcode = GetCodeFromImage(imageBytes, rectangle, true);
                
                if (barcode != null && barcode.Text != "")
                {
                    var bc = new Barcode();
                    bc.Number = barcode.Text;
                    bc.Type = barcode.BarcodeFormat == BarcodeFormat.CODE_39 ? BarcodeType.Code39 : BarcodeType.Code128;

                    return bc;
                }

                return new Barcode() { Type = BarcodeType.Empty, Number = "" };
            }
            catch (Exception)
            {
                return new Barcode() { Type = BarcodeType.Empty, Number = "" };
            }
        }

        private static Result GetCodeFromImage(byte[] imageBytes, DTO.Rectangle rectangle, bool rotate = false)
        {
            var btmap = SKBitmap.Decode(imageBytes);

            if (rotate)
            {
                //Rotate the image 180 degrees, and replace the bitmap with the rotated image
                var rotated = new SKBitmap(btmap.Width, btmap.Height);
                using (var surface = new SKCanvas(rotated))
                {
                    surface.Translate(rotated.Width, rotated.Height);
                    surface.RotateDegrees(180);
                    surface.DrawBitmap(btmap, 0, 0);
                }

                btmap = rotated;
            }

            var subBitmap = btmap;
            var cropArea = new SKRectI();
            if (btmap.Height > 150)
            {
                if(rectangle.Width == 0)
                {
                    var heightOfSelection = (int)(btmap.Height / 7);
                    rectangle.Height = heightOfSelection;
                    rectangle.StartX = 0;
                    rectangle.StartY = 0;
                    rectangle.Width = btmap.Width - 1;

                    cropArea = new SKRectI((int)rectangle.StartX, (int)rectangle.StartY, (int)rectangle.Width, (int)rectangle.Height);
                }
                else
                {
                    var widthFactor = btmap.Width / rectangle.RefImageWidth;
                    var heightFactor = btmap.Height / rectangle.RefImageHeight;

                    var calcStartX = rectangle.StartX * widthFactor;
                    var calcStartY = rectangle.StartY * heightFactor;
                    var calcWidth = rectangle.Width * widthFactor;
                    var calcHeight = rectangle.Height * heightFactor;

                    cropArea = new SKRectI((int)calcStartX, (int)calcStartY, (int)calcWidth, (int)calcHeight);
                }

                var img = SKImage.FromBitmap(btmap);
                var subset = img.Subset(cropArea);
                subBitmap = SKBitmap.FromImage(subset);

                img.Dispose();
                subset.Dispose();
            }
            
            var reader = new ZXing.BarcodeReader();
            var barcodes = reader.Decode(subBitmap);

            btmap.Dispose();
            subBitmap.Dispose();

            return barcodes;
        }
    }
}
