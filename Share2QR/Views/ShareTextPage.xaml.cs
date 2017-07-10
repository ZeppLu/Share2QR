using Share2QR.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Share2QR.Views
{
    public sealed partial class ShareTextPage : Page
    {
        public ShareTextPage()
        {
            Model = new ShareTextViewModel(Dispatcher);
            this.InitializeComponent();
        }

        public ShareTextViewModel Model;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await Model.OnNavigatedToAsync(e);
        }
    }
}
