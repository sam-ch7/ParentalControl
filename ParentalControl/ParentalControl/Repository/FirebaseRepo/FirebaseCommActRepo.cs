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

namespace ParentalControl.Repository.FirebaseRepo
{
    public class FirebaseCommActRepo :BaseViewModel, ICommunicationActivitiesRepository
    {
        private FirebaseClient firebase;
        User user;
        public FirebaseCommActRepo(FirebaseClient firebase)
        {
            this.firebase = firebase;
            user = AuthenticationService.GetCurrentUser();
        }
        public async Task<List<ChildCallLog>> GetCallLogs(string DeviceID)
        {
            var res =await firebase.Child(user.ID).
                Child(DeviceID).Child("CallLogs")
                .OnceAsync<ChildCallLog>();
            var List = res.Select(item =>
                    new ChildCallLog
                    {
                        ContactName=item.Object.ContactName,
                        CallNumber = item.Object.CallNumber,
                        CallDate = item.Object.CallDate,
                        CallType = item.Object.CallType,
                        Duration = item.Object.Duration
                    }).ToList();
            return List;
        }

        public async void UploadCallLogsAsync()
        {
            var CLs = DependencyService.Get<ICallLogsService>().GetCallLogs();
            var UpList = await GetCallLogs(SpecialDeviceInfo.GetDeviceID());
            foreach(var cl in CLs)
            {
                if (UpList.Where(a => a.CallDate == cl.CallDate).Count() > 0)
                {
                }
                else
                {
                    await firebase.Child(user.ID).Child(SpecialDeviceInfo.GetDeviceID()).Child("CallLogs")
                    .PostAsync(cl);
                }
            }
            //foreach (var cl in logs)
            //{
            //    if (UpList.Where(a => a. == app.PackageName).Count() > 0)
            //    {
            //    }
            //}
        }
    }
}
