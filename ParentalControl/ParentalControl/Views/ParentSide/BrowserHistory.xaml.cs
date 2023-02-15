//using Plugin.Permissions;
//using Plugin.Permissions.Abstractions;
using ParentalControl.Models;
using ParentalControl.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using PermissionStatus = Xamarin.Essentials.PermissionStatus;
//using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace ParentalControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BrowserHistory : ContentPage
    {
        public BrowserHistory()
        {
            InitializeComponent();
            //LoadCallLogs();
            //LoadBrowsingHistory();
            LoadInstalledApplications();

            MLV.ItemsSource = ApplicationsCollect;
        }
        ObservableCollection<string> BrowsingHistoryCollect = new ObservableCollection<string>();
        ObservableCollection<ChildCallLog> CallLogsCollect = new ObservableCollection<ChildCallLog>();
        ObservableCollection<Contact> ContactsCollect = new ObservableCollection<Contact>();
        ObservableCollection<Models.Application> ApplicationsCollect = new ObservableCollection<Models.Application>();

        //ObservableCollection<ActivityManager.RunningAppProcessInfo> RunApplicationsCollect = new ObservableCollection<ActivityManager.RunningAppProcessInfo>();
        public async Task<PermissionStatus> CheckAndContactsReadPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
            if (status == PermissionStatus.Granted)
            {
                return status;
            }
            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }
            status = await Permissions.RequestAsync<Permissions.ContactsRead>();
            return status;
        }
        /// <summary>
        /// UninstallPackage
        /// </summary>
        async void LoadBrowsingHistory()
        {
            var res = DependencyService.Get<IBrowsingHistoryService>().UninstallPackage("com.cpuid.cpu_z");
            if (res==true)
            {
                await DisplayAlert("Done", "Done", "OK");
            }
            try
            {
                //Browser.OpenAsync("https://docs.microsoft.com/en-us/xamarin/essentials/open-browser?tabs=android");
                LoadingContactsLabel.IsVisible = true;
                LoadingContactsAI.IsVisible = true;
                // cancellationToken parameter is optional
                //var cancellationToken = default(CancellationToken);
                var BH = DependencyService.Get<IBrowsingHistoryService>().GetBrowsingHistory();
                foreach (var bh in BH)
                {
                    BrowsingHistoryCollect.Add(bh);
                }

                LoadingContactsLabel.IsVisible = false;
                LoadingContactsAI.IsVisible = false;
                return;
                //await DisplayAlert("Need location",contactsCollect.FirstOrDefault().DisplayName, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        async void LoadContacts()
        {
            var status = await CheckAndContactsReadPermission();
            if(status == PermissionStatus.Granted)
            {
                try
                {
                    LoadingContactsLabel.IsVisible = true;
                    LoadingContactsAI.IsVisible = true;
                    // cancellationToken parameter is optional
                    //var cancellationToken = default(CancellationToken);
                    var contacts = await Contacts.GetAllAsync();
                    if (contacts == null)
                    {
                        return;
                    }
                    foreach (var contact in contacts)
                    {
                        ContactsCollect.Add(contact);
                    }
                    LoadingContactsLabel.IsVisible = false;
                    LoadingContactsAI.IsVisible = false;
                    return;
                    //await DisplayAlert("Need location",contactsCollect.FirstOrDefault().DisplayName, "OK");
                }
                catch (Exception ex)
                {
                    // Handle exception here.
                }
            }
            else
            {
                await DisplayAlert("Contacts Not Available", "Permission To Read Contacts Not Granted", "OK");
            }
            
        }
        async void LoadCallLogs()
        {
            //await DisplayAlert("Need location", CLs.FirstOrDefault().CallNumber, "OK");
            try
            {
                LoadingContactsLabel.IsVisible = true;
                LoadingContactsAI.IsVisible = true;
                // cancellationToken parameter is optional
                //var cancellationToken = default(CancellationToken);
                var CLs = DependencyService.Get<ICallLogsService>().GetCallLogs();
                foreach (var cLog in CLs)
                {
                    CallLogsCollect.Add(cLog);
                }

                LoadingContactsLabel.IsVisible = false;
                LoadingContactsAI.IsVisible = false;
                return;
                //await DisplayAlert("Need location",contactsCollect.FirstOrDefault().DisplayName, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        async void LoadInstalledApplications()
        {
            try
            {
                LoadingContactsLabel.IsVisible = true;
                LoadingContactsAI.IsVisible = true;
                // cancellationToken parameter is optional
                //var cancellationToken = default(CancellationToken);
                var IAs = DependencyService.Get<IAppActivitiesService>().GetInstalledApplications();
                //DependencyService.Get<IAppActivitiesService>().LockApplication();
                foreach (var IA in IAs)
                {
                    ApplicationsCollect.Add(IA);
                }

                LoadingContactsLabel.IsVisible = false;
                LoadingContactsAI.IsVisible = false;
                return;
                //await DisplayAlert("Need location",contactsCollect.FirstOrDefault().DisplayName, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

    }
}