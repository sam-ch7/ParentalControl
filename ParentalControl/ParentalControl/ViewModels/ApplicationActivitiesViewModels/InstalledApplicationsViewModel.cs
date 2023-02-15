using ParentalControl.Models;
using ParentalControl.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ParentalControl.ViewModels.ApplicationActivitiesViewModels
{
    public class InstalledApplicationsViewModel:BaseViewModel
    {
        public ObservableCollection<Models.Application> InstalledApplications { get; }
        public Command LoadApplicationsCommand { get; }
        public Command<Models.Application> BlockAppCommand { get; }
        public Command UploadInstalledAppsCommand { get; }
        //public Command BlockAppCommand { get; }
        internal void OnAppearing()
        {
            IsBusy = true;
        }
        public InstalledApplicationsViewModel()
        {
            InstalledApplications = new ObservableCollection<Models.Application>();
            LoadApplicationsCommand = new Command(async () => await ExecLoadInstalledApplicationsCommand());
            UploadInstalledAppsCommand = new Command(async () => await ExecUploadInstalledAppsCommand());
            BlockAppCommand = new Command<Models.Application>(ExecBlockAppCommand);
        }

        public async void ExecBlockAppCommand(Models.Application app)
        {
            base.SwitchDatabase();
            var res = false;
            if (!app.IsBlocked)
            {
                res = await Repository.ApplicationActivitiesRepository.AddApplicationToBlockList(app);
            }
            else
            {
                Repository.ApplicationActivitiesRepository.RemoveAppFromBlockList(app);
                res = true;
            }
            if (res)
            {
                await ExecLoadInstalledApplicationsCommand();
            }
            
            
        }

        async Task ExecLoadInstalledApplicationsCommand()
        {
            IsBusy = true;
            base.SwitchDatabase();
            try
            {
                InstalledApplications.Clear();
                var IAs = await Repository.ApplicationActivitiesRepository.GetInstalledApplicationsList(SelectedChildDevice.ID);
                foreach (var app in IAs)
                {
                    InstalledApplications.Add(app);
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
        
        public async Task ExecUploadInstalledAppsCommand()
        {
            try
            {
                var IAs = AppActivitiesService.GetInstalledApplications();
                Repository.ApplicationActivitiesRepository.UploadInstalledApplications(IAs);
            }
            catch
            {

            }
        }
    }
}
