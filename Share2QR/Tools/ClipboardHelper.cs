using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace Share2QR.Tools
{
    static class ClipboardHelper
    {
        public static async Task<bool> CopyImageAsync(SoftwareBitmap bitmap)
        {
            // Not familiar with C#'s memory management, but clipboard content will be inaccessible
            // as soon as stream gets disposed
            //using (var stream = new InMemoryRandomAccessStream())
            //{
            var stream = new InMemoryRandomAccessStream();

            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
            encoder.SetSoftwareBitmap(bitmap);
            await encoder.FlushAsync();

            var dataPackage = new DataPackage()
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            dataPackage.SetBitmap(RandomAccessStreamReference.CreateFromStream(stream));
            Clipboard.SetContent(dataPackage);
            //}
            return true;
        }
    }
}
