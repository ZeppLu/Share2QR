using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Share2QR.Tools
{
    static class FileHelper
    {
        public static async Task<string> SaveImageAsync(SoftwareBitmap bitmap)
        {
            var picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            // TODO: more formats
            // https://github.com/xyzzer/WinRTXamlToolkit/blob/master/WinRTXamlToolkit/Imaging/WriteableBitmapSaveExtensions.cs
            picker.FileTypeChoices.Add("PNG images", new[] { ".png" });
            var file = await picker.PickSaveFileAsync();

            if (file == null)
            {
                // file == null : operation cancelled
                // TODO: throw an exception
                return "";
            }

            var filename = file.Name;
            var encoderGuid = BitmapEncoder.PngEncoderId;
            // TODO: change encoderGuid according to filename extension

            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(encoderGuid, stream);
                encoder.SetSoftwareBitmap(bitmap);
                await encoder.FlushAsync();
            }

            return filename;
        }
    }
}
