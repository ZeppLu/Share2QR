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
    public sealed class ShareTextViewModel : INotifyPropertyChanged
    {
        public ShareTextViewModel(CoreDispatcher dispatcher)
        {
            _Dispatcher = dispatcher;
            QrCodeBitmap = null;
            BottomCommandBarText = "";
            IsBusy = false;
        }

#region Properties
        // View's dispatcher is required to update UI element, which is bind to some property
        private CoreDispatcher _Dispatcher { get; set; }

        private SoftwareBitmap _QrCodeBitmap;
        public SoftwareBitmap QrCodeBitmap
        {
            get { return _QrCodeBitmap; }
            set
            {
                if (value != _QrCodeBitmap)
                {
                    _QrCodeBitmap = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("QrCodeSource");
                }
            }
        }

        public ImageSource QrCodeSource
        {
            get
            {
                var qrCodeSource = new SoftwareBitmapSource();
                qrCodeSource.SetBitmapAsync(QrCodeBitmap);
                return qrCodeSource;
            }
        }

        private string _BottomCommandBarText;
        public string BottomCommandBarText
        {
            get { return _BottomCommandBarText; }
            set
            {
                if (value != _BottomCommandBarText)
                {
                    _BottomCommandBarText = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                _IsBusy = value;
                NotifyPropertyChanged();
            }
        }
#endregion

        public async Task OnNavigatedToAsync(NavigationEventArgs e)
        {
            var shareOperation = e.Parameter as ShareOperation;

            var text = "";
            if (shareOperation.Data.Contains(StandardDataFormats.WebLink))
            {
                text = (await shareOperation.Data.GetWebLinkAsync()).ToString();
            }
            else if (shareOperation.Data.Contains(StandardDataFormats.Text))
            {
                text = await shareOperation.Data.GetTextAsync();
            }
            // TODO: vCard format

            QrCodeBitmap = QrCodeHelper.Encode(text, 256, 256);
        }

        public async Task SaveAsync(object sender, RoutedEventArgs e)
        {
            if (QrCodeBitmap == null)
            {
                return;
            }

            IsBusy = true;
            BottomCommandBarText = "Saving...";
            await FileHelper.SaveImageAsync(QrCodeBitmap).ContinueWith(async (antecedent) =>
            {
                // Dispatcher is needed to modify data in another thread
                await _Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    if (String.IsNullOrEmpty(antecedent.Result))
                    {
                        BottomCommandBarText = "Cancelled";
                    }
                    else
                    {
                        BottomCommandBarText = antecedent.Result + " saved ;)";
                    }
                    IsBusy = false;
                    await Task.Delay(3000);
                    BottomCommandBarText = "";
                });
            });
        }

        public async Task CopyAsync(object sender, RoutedEventArgs e)
        {
            if (QrCodeBitmap == null)
            {
                return;
            }

            await ClipboardHelper.CopyImageAsync(QrCodeBitmap).ContinueWith(async (ancedent) =>
            {
                await _Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (ancedent.Result)
                    {
                        BottomCommandBarText = "Copied";
                    }
                    else
                    {
                        BottomCommandBarText = "Failed to copy";
                    }
                });
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
