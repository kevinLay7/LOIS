using SkiaSharp;
using System.IO;

namespace LOIS.BLL.Imaging
{
    public class ImageCompressor
    {
        public static void CompressImage(ref byte[] image)
        {
            var bitmap = SKBitmap.Decode(image);
            var img = SKImage.FromBitmap(bitmap);
            var data = img.Encode(SKEncodedImageFormat.Jpeg, 40);
            var ms = new MemoryStream();
            data.SaveTo(ms);
            image = ms.ToArray();
            ms.Dispose();
        }
    }
}
