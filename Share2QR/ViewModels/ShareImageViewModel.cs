using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Graphics.Imaging;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

using Share2QR.Tools;

namespace Share2QR.ViewModels
{
    public sealed class ShareImageViewModel : INotifyPropertyChanged
    {
#region Properties
        private CoreDispatcher _Dispatcher { get; set; }

        private BitmapImage _SharedImage;
        public BitmapImage SharedImage
        {
            get { return _SharedImage; }
            set
            {
                if (value != _SharedImage)
                {
                    _SharedImage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _DecodedText;
        public string DecodedText
        {
            get { return _DecodedText; }
            set
            {
                if (value != _DecodedText)
                {
                    _DecodedText = value;
                    NotifyPropertyChanged();
                }
            }
        }
#endregion

        public ShareImageViewModel(CoreDispatcher dispatcher)
        {
            _Dispatcher = dispatcher;
            SharedImage = new BitmapImage();
            DecodedText = "";
        }

        public async Task OnNavigatedToAsync(NavigationEventArgs e)
        {
            var shareOperation = e.Parameter as ShareOperation;

            if (shareOperation.Data.Contains(StandardDataFormats.Bitmap))
            {
                var streamRef = await shareOperation.Data.GetBitmapAsync();
                var stream = await streamRef.OpenReadAsync();

                SharedImage.SetSource(stream);
                NotifyPropertyChanged("SharedImage");

                //var source = (BitmapSource)SharedImage;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
