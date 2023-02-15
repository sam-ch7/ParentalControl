using ParentalControl.ViewModels.OnlineActivitiesViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParentalControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BlockedWebsitesPage : ContentPage
    {
        BlockedWebsitesViewModel ViewModel;
        public BlockedWebsitesPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new BlockedWebsitesViewModel();
            //ViewModel.LoadWebsitesCommand;
        }
        protected override void OnAppearing()
        {
            ViewModel.OnAppearing();
        }
    }
}