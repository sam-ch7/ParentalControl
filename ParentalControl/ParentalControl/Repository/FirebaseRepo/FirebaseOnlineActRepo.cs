using Firebase.Database;
using Firebase.Database.Query;
using ParentalControl.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ParentalControl.ViewModels.Base;
using Xamarin.Essentials;

namespace ParentalControl.Repository.FirebaseRepo
{
    public class FirebaseOnlineActRepo : BaseViewModel,IOnlineActivitiesRepository
    {
        FirebaseClient fbc;
        User user;
        public FirebaseOnlineActRepo(FirebaseClient firebaseClient)
        {
            fbc = firebaseClient;
            user = AuthenticationService.GetCurrentUser();
        }
        public async Task<bool> AddBlockedWebsite(Website website)
        {
            try
            {
                //var user = AuthenticationService.GetCurrentUser();
                await fbc.Child(user.ID).Child(SelectedChildDevice.ID).Child("BlockedWebsites")
                .PostAsync(website);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Website>> GetBlockedWebsites(string DeviceID)
        {
            try
            {
                //var user = AuthenticationService.GetCurrentUser();
                var res =await fbc.Child(user.ID).Child(DeviceID)
                    .Child("BlockedWebsites")
                    .OnceAsync<Website>();
                var BlockedWebsitesList = res.Select(item =>
                    new Website
                    {
                        Name = item.Object.Name,
                        URL = item.Object.URL
                    }).ToList();
                return BlockedWebsitesList;
            }
            catch
            {
                var list = new List<Website>();
                list.Add(new Website { Name = "Null", URL = "Null" });
                return list;
            }
        }
        public async void RemoveFromBlocklist(string URL)
        {
           var ToBeDeleted = fbc.Child(user.ID).Child(SelectedChildDevice.ID).Child("BlockedWebsites")
                .OnceAsync<Website>().Result.Where(a => a.Object.URL == URL).FirstOrDefault();
           await fbc.Child(user.ID).Child(SelectedChildDevice.ID)
              .Child("BlockedWebsites")
              .Child(ToBeDeleted.Key)
              .DeleteAsync();

        }
    }
}
