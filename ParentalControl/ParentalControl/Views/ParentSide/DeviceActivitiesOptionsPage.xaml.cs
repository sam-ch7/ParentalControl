using ParentalControl.Models;
using ParentalControl.Services;
using ParentalControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParentalControl.Views.ParentSide
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceActivitiesOptionsPage : ContentPage
    {
        public DeviceActivitiesOptionsPage()
        {
            InitializeComponent();
            //BindingContext=SelectedChildDevice
            NameL.Text = SelectedChildDevice.Name;
            PlatformL.Text = SelectedChildDevice.Platform;
            VersionL.Text = SelectedChildDevice.VersionString;
            ModelL.Text = SelectedChildDevice.Model;
            ManufacturerL.Text = SelectedChildDevice.Manufacturer;
            AccDis.IsVisible = !SelectedChildDevice.IsAccessibilityServiceEnabled;
            AccEna.IsVisible = SelectedChildDevice.IsAccessibilityServiceEnabled;

            BindingContext = new DAOPViewModel();
            //LoadCallLogs();
        }
        async void LoadCallLogs()
        {
            //await DisplayAlert("Need location", CLs.FirstOrDefault().CallNumber, "OK");
            try
            {
                
                // cancellationToken parameter is optional
                //var cancellationToken = default(CancellationToken);
                var CLs = DependencyService.Get<ICallLogsService>().GetCallLogs();
                foreach (var cLog in CLs)
                {
                    Console.WriteLine("Type "+cLog.CallType+" Number "+ cLog.CallNumber+" Date "+ cLog.CallDate+" Duration "+ cLog.Duration);
                }

                
                //await DisplayAlert("Need location",contactsCollect.FirstOrDefault().DisplayName, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async Task AppActBt_Clicked()
        {
            Console.WriteLine("lll");
            await Shell.Current.GoToAsync(nameof(InstalledAppsPage));
        }

        private async void OnlineActBt_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(BlockedWebsitesPage));
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private async void CommAct_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CommunicationActivitiesOptionsPage));
        }

        private async void LocAct_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(MapView));
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var model = (Grid)sender;
            model.BackgroundColor = Color.LightGray;
        }
    }
}