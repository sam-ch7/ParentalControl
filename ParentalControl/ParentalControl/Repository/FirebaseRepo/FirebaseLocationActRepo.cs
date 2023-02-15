using Firebase.Database;
using Firebase.Database.Query;
using ParentalControl.Models;
using ParentalControl.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ParentalControl.Repository.FirebaseRepo
{
    public class FirebaseLocationActRepo :BaseViewModel, ILocationActivitiesRepository
    {
        private FirebaseClient firebase;
        User user;
        public FirebaseLocationActRepo(FirebaseClient firebase)
        {
            this.firebase = firebase;
            user = AuthenticationService.GetCurrentUser();
        }
        public Location GetLocation()
        {
            try
            {
                return firebase.Child(user.ID).
                Child(SelectedChildDevice.ID).Child("Location")
                .OnceAsync<Location>().Result.Select(item =>
                    new Location
                    {
                        Latitude = item.Object.Latitude,
                        Longitude = item.Object.Longitude,
                        Timestamp = item.Object.Timestamp
                    }).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task UploadLocation(Location location)
        {
            try
            {
                var ToBeUpdated = firebase.Child(user.ID).Child(SpecialDeviceInfo.GetDeviceID()).Child("Location")
                .OnceAsync<Location>().Result.FirstOrDefault();
                if (ToBeUpdated != null)
                {
                    await firebase.Child(user.ID).Child(SpecialDeviceInfo.GetDeviceID())
                    .Child("Location")
                    .Child(ToBeUpdated.Key)
                    .PutAsync(new Location() { Latitude = location.Latitude, Longitude = location.Longitude,Timestamp=DateTimeOffset.Now });
                    return;
                }
                await firebase.Child(user.ID).Child(SpecialDeviceInfo.GetDeviceID())
                  .Child("Location")

                  .PostAsync(new Location() { Latitude = location.Latitude, Longitude = location.Longitude });
                
            }
            catch(Exception e)
            {

                Console.WriteLine(e.Message);
            }
            
        }
    }
}
