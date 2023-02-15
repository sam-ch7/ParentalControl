using ParentalControl.Repository;
using ParentalControl.Repository.FirebaseRepo;
using ParentalControl.Services;
using ParentalControl.ViewModels;
using ParentalControl.Views;
using ParentalControl.Views.ChildSide;
using ParentalControl.Views.ParentSide;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
namespace ParentalControl
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public IParentalControlRepository Repository = DependencyService.Get<FirebaseParentalControlRepo>();
        IAuthenticationService AuthenticationService => DependencyService.Get<IAuthenticationService>();
        public ICurrentDeviceService CurrentDeviceService = DependencyService.Get<ICurrentDeviceService>();
        IServicesControl ServiceControl = DependencyService.Get<IServicesControl>();
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(ChildDevicesPage), typeof(ChildDevicesPage));
            Routing.RegisterRoute(nameof(CallLogsListPage), typeof(CallLogsListPage));
            Routing.RegisterRoute(nameof(MapView), typeof(MapView));
            Routing.RegisterRoute(nameof(CommunicationActivitiesOptionsPage), typeof(CommunicationActivitiesOptionsPage));
            Routing.RegisterRoute(nameof(BlockedWebsitesPage), typeof(BlockedWebsitesPage));
            Routing.RegisterRoute(nameof(DeviceActivitiesOptionsPage), typeof(DeviceActivitiesOptionsPage));
            Routing.RegisterRoute(nameof(InstalledAppsPage), typeof(InstalledAppsPage));
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(SelectDevicePage), typeof(SelectDevicePage));
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(BrowserHistory), typeof(BrowserHistory));
            Routing.RegisterRoute(nameof(AddBlockedWebsite), typeof(AddBlockedWebsite));
            //Routing.RegisterRoute(nameof(Dashboard), typeof(Dashboard));

        }
        public async void ConfigureDevice()
        {
            var isparentdevice = await CurrentDeviceService.IsParentDevice();
            ChildSideDashboardItem.IsVisible = !isparentdevice;
            ChildDevicesItem.IsVisible = isparentdevice;
            
        }

        private async void Shell_Navigated(object sender, ShellNavigatedEventArgs e)
        {
            try
            {
                var p = App.Current.MainPage.Navigation.NavigationStack.LastOrDefault();
                if (!AuthenticationService.IsSignedIn() &&
                    p.GetType() != typeof(SignUpPage))
                {
                    Shell.Current.GoToAsync("//LoginPage");
                    return;
                }
            }
            catch
            {
                if (!AuthenticationService.IsSignedIn()
                    )
                {
                    Shell.Current.GoToAsync("//LoginPage");
                    return;
                }
            }
            if (AuthenticationService.IsSignedIn())
            {
                HeaderGrid.BindingContext = new AboutViewModel();
            }
            var isparentdevice = await CurrentDeviceService.IsParentDevice();
            if (!isparentdevice)
            {
                var permission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.LocationAlways>();

                if (permission == Xamarin.Essentials.PermissionStatus.Denied)
                {
                    Console.WriteLine("Denied---------------------------");
                    return;
                }
            }
            else
            {
                ServiceControl.StopLocationService();
            }
            //var name = this.GetType().Name;
            //Console.WriteLine(name);

            //if (p.GetType() == typeof(SelectDevicePage))
            //{
            //    ConfigureItem.IsVisible = true;
            //    return;
            //}
            //else if (p.GetType() != typeof(ConfigureDevicePage))
            //{
            //    ConfigureItem.IsVisible = false;
            //}
            
            ConfigureDevice();
        }

        private async void LogoutItem_Clicked(object sender, EventArgs e)
        {
            var isparentdevice = await CurrentDeviceService.IsParentDevice();
            if (!isparentdevice)
            {
                Repository.ChildDevicesRepository.OnLogout();
            }
            AuthenticationService.SignOut();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
