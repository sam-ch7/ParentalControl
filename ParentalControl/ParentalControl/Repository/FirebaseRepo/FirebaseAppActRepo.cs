using Firebase.Database;
using Firebase.Database.Query;
using ParentalControl.Models;
using ParentalControl.Services;
using ParentalControl.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Application = ParentalControl.Models.Application;

namespace ParentalControl.Repository.FirebaseRepo
{
    public class FirebaseAppActRepo : BaseViewModel, IApplicationActivitiesRepository
    {
        private FirebaseClient firebase;
        User user;
        public FirebaseAppActRepo(FirebaseClient firebase)
        {
            this.firebase = firebase;
            user = AuthenticationService.GetCurrentUser();
        }

        public async Task<bool> AddApplicationToBlockList(Application app)
        {
            try
            {
                await firebase.Child(user.ID).Child(SelectedChildDevice.ID)
                .Child("BlockedApps").PostAsync(app);
                var res = await firebase.Child(user.ID).Child(SelectedChildDevice.ID).Child("InstalledApplications")
                    .OnceAsync<Application>();
                var ToBeUpdated = res.Where(a => a.Object.PackageName == app.PackageName).FirstOrDefault();
                await firebase.Child(user.ID).Child(SelectedChildDevice.ID)
                  .Child("InstalledApplications")
                  .Child(ToBeUpdated.Key)
                  .PutAsync(new Application() { Name = app.Name, PackageName = app.PackageName, IsBlocked = true });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void RemoveAppFromBlockList(Application app)
        {
            var ToBeUpdated = firebase.Child(user.ID).Child(SelectedChildDevice.ID).Child("InstalledApplications")
                .OnceAsync<Application>().Result.Where(a => a.Object.PackageName == app.PackageName).FirstOrDefault();
            firebase.Child(user.ID).Child(SelectedChildDevice.ID)
              .Child("InstalledApplications")
              .Child(ToBeUpdated.Key)
              .PutAsync(new Application() { Name = app.Name, PackageName = app.PackageName, IsBlocked = false });
            var ToBeDeleted = firebase.Child(user.ID).Child(SelectedChildDevice.ID).Child("BlockedApps")
                .OnceAsync<Application>().Result.Where(a => a.Object.PackageName == app.PackageName).FirstOrDefault();
            firebase.Child(user.ID).Child(SelectedChildDevice.ID)
              .Child("BlockedApps")
              .Child(ToBeDeleted.Key)
              .DeleteAsync();

        }
        public async Task<List<Application>> GetBlockedApplications()
        {
            var res = await firebase.Child(user.ID).Child(SpecialDeviceInfo.GetDeviceID()).Child("BlockedApps")
                .OnceAsync<Application>();
            var List = res.Select(item =>
                    new Application
                    {
                        Name = item.Object.Name,
                        PackageName = item.Object.PackageName
                    }).ToList();
            return List;
        }

        public async Task<List<Application>> GetInstalledApplicationsList(string ID)
        {
            var res = await firebase.Child(user.ID).
                Child(ID).Child("InstalledApplications")
                .OnceAsync<Application>();
            var List = res.Select(item =>
                    new Application
                    {
                        Name = item.Object.Name,
                        PackageName = item.Object.PackageName,
                        IsBlocked = item.Object.IsBlocked
                    }).ToList();
            return List;
        }

        

        public async void UploadInstalledApplications(List<Application> applications = null)
        {
            if(ActionControl.IsActive)
            {
                return;
            }
            ActionControl.IsActive = true;
            var UpList = await GetInstalledApplicationsList(SpecialDeviceInfo.GetDeviceID());
            foreach (var app in applications)
            {
                if (UpList.Where(a=>a.PackageName==app.PackageName).Count()>0)
                {
                }
                else
                {
                    await firebase.Child(user.ID).Child(SpecialDeviceInfo.GetDeviceID()).Child("InstalledApplications")
                    .PostAsync(app);
                }
            }
        }
    }
}
