//using Android.Content;
//using Android.Provider;
using ParentalControl.Models;
using ParentalControl.ViewModels.Base;
using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace ParentalControl.ViewModels
{
    public class CallLogsViewModel:BaseViewModel
    {
        public ObservableCollection<ChildCallLog> CallLogs { get; }
        public Command LoadCallLogsCommand { get; }
        public CallLogsViewModel()
        {
            CallLogs = new ObservableCollection<ChildCallLog>();
            LoadCallLogsCommand = new Command(async () => await GetCallLogs());
        }
        internal void OnAppearing()
        {
            IsBusy = true;
        }
        public async Task GetCallLogs()
        {
            IsBusy = true;
            base.SwitchDatabase();
            try
            {
                CallLogs.Clear();
                var CLs = await Repository.CommunicationActivitiesRepository.GetCallLogs(SelectedChildDevice.ID);
                foreach (var cl in CLs)
                {
                    CallLogs.Add(cl);
                    Console.WriteLine(cl.CallNumber);
                }

                //base.SwitchToOfflineDB();
                //Repository.ApplicationActivitiesRepository.UploadInstalledApplications(IAs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
