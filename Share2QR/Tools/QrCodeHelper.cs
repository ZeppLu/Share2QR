using System;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using ZXing;
using ZXing.Common;

namespace Share2QR.Tools
{
    static class QrCodeHelper
    {
        public static SoftwareBitmap Encode(string text, int height, int width)
        {
            if (String.IsNullOrEmpty(text))
            {
                // TODO: exception
                return null;
            }

            var qrCodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = 0,
                    PureBarcode = false,
                },
            };

            var writeableBitmap = qrCodeWriter.Write(text);
            return SoftwareBitmap.CreateCopyFromBuffer(
                writeableBitmap.PixelBuffer,
                BitmapPixelFormat.Bgra8,
                writeableBitmap.PixelWidth,
                writeableBitmap.PixelHeight,
                BitmapAlphaMode.Ignore
            );
        }

        //public static string Decode(BitmapSource source)
        //{
        //    var reader = new BarcodeReader();
        //    reader.Decode(source);
        //}
    }
}
