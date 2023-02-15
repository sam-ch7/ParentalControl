using ParentalControl.ViewModels.Base;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ParentalControl.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            var user = AuthenticationService.GetCurrentUser();
            
            Title = "Profile";
            Email = user.Email;
            DisplayName = user.Username;
            OpenWebCommand = new Command(Refresh/*async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart")*/);
            Configure();
        }
        string email = string.Empty;
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }
        string displayName = string.Empty;
        public string DisplayName
        {
            get { return displayName; }
            set { SetProperty(ref displayName, value); }
        }
        //public string Email { get; set; }
        //public string DisplayName { get; set; }
        //public string Email { get; set; }
        async void Configure()
        {
            if (!await CurrentDeviceService.IsParentDevice())
            {
                Repository.ChildDevicesRepository.AddChildDevice();
                try
                {
                    Console.WriteLine("--------------------------------------------------Child");
                    var IAs = AppActivitiesService.GetInstalledApplications();
                    Repository.ApplicationActivitiesRepository.UploadInstalledApplications(IAs);
                    Repository.CommunicationActivitiesRepository.UploadCallLogsAsync();
                }
                catch
                {

                }
            }
        }
        public ICommand OpenWebCommand { get; }
        async void Refresh()
        {
            var user = AuthenticationService.GetCurrentUser();
            Title = user.Email;
            await Application.Current.MainPage.DisplayAlert("User Name", user.Username, "Cancel");
        }
    }
}