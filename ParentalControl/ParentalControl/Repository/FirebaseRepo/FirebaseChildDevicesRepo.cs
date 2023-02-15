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
using Xamarin.Forms;

namespace ParentalControl.Repository.FirebaseRepo
{
    public class FirebaseChildDevicesRepo :BaseViewModel, IChildDevicesRepository
    {
        private FirebaseClient firebase;
        User user;
        
        public FirebaseChildDevicesRepo(FirebaseClient firebase)
        {
            this.firebase = firebase;
            user = AuthenticationService.GetCurrentUser();
        }
        public async void AddChildDevice()
        {
            try
            {
                if (await CurrentDeviceService.IsParentDevice())
                {
                    return;
                }
                var exists = firebase.Child(user.ID)
                    .Child("ChildDevices").OnceAsync<ChildDevice>().Result.Where(cd => cd.Object.ID == SpecialDeviceInfo.GetDeviceID()).FirstOrDefault();
                if (exists != null)
                {
                    await firebase.Child(user.ID).Child("ChildDevices")
                   .Child(exists.Key)
                   .PutAsync(new ChildDevice()
                   {
                       ID = SpecialDeviceInfo.GetDeviceID(),
                       Name = DeviceInfo.Name,
                       Platform = DeviceInfo.Platform.ToString(),
                       Manufacturer = DeviceInfo.Manufacturer,
                       Model = DeviceInfo.Model,
                       VersionString = DeviceInfo.VersionString,
                       IsAccessibilityServiceEnabled = SpecialDeviceInfo.IsAccessibilityServiceEnabled(),
                       IsLoggedIn = AuthenticationService.IsSignedIn()
                   });
                    return;
                }
                await firebase.Child(user.ID)
                    .Child("ChildDevices")
                    .PostAsync(new ChildDevice
                    {
                        ID = SpecialDeviceInfo.GetDeviceID(),
                        Name = DeviceInfo.Name,
                        Platform = DeviceInfo.Platform.ToString(),
                        Manufacturer = DeviceInfo.Manufacturer,
                        Model = DeviceInfo.Model,
                        VersionString = DeviceInfo.VersionString,
                        IsAccessibilityServiceEnabled = SpecialDeviceInfo.IsAccessibilityServiceEnabled(),
                        IsLoggedIn = AuthenticationService.IsSignedIn()
                    });
            }
            catch
            {

            }
        }
        public void OnLogout()
        {
            var ToBeUpdated = firebase.Child(user.ID)
                .Child("ChildDevices").OnceAsync<ChildDevice>().Result.Where(cd => cd.Object.ID == SpecialDeviceInfo.GetDeviceID()).FirstOrDefault();
            firebase.Child(user.ID).Child("ChildDevices")
              .Child(ToBeUpdated.Key)
              .PutAsync(new ChildDevice()
              {
                  ID = SpecialDeviceInfo.GetDeviceID(),
                  Name = DeviceInfo.Name,
                  Platform = DeviceInfo.Platform.ToString(),
                  Manufacturer = DeviceInfo.Manufacturer,
                  Model = DeviceInfo.Model,
                  VersionString = DeviceInfo.VersionString,
                  IsAccessibilityServiceEnabled = SpecialDeviceInfo.IsAccessibilityServiceEnabled(),
                  IsLoggedIn = false
              });
        }
        public async Task<List<ChildDevice>> GetChildDevices()
        {
            var res =await firebase.Child(user.ID).Child("ChildDevices").OnceAsync<ChildDevice>();
            return res.Select(item =>
                    new ChildDevice
                    {
                        ID=item.Object.ID,
                        Name = item.Object.Name,
                        Platform = item.Object.Platform,
                        Manufacturer = item.Object.Manufacturer,
                        Model = item.Object.Model,
                        VersionString = item.Object.VersionString,
                        IsAccessibilityServiceEnabled = item.Object.IsAccessibilityServiceEnabled,
                        IsLoggedIn=item.Object.IsLoggedIn
                    }).ToList();
        }

        public void RemoveChildDevice()
        {
            throw new NotImplementedException();
        }
    }
}
