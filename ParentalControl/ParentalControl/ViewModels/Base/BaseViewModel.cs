using ParentalControl.Models;
using ParentalControl.Repository;
using ParentalControl.Repository.FirebaseRepo;
using ParentalControl.Repository.SQLiteRepo;
using ParentalControl.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ParentalControl.ViewModels.Base
{
    public class BaseViewModel : BindableBase
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        public IParentalControlRepository Repository { get; set; }
        public IAuthenticationService AuthenticationService = DependencyService.Get<IAuthenticationService>();
        public IAppActivitiesService AppActivitiesService = DependencyService.Get<IAppActivitiesService>();
        public ICurrentDeviceService CurrentDeviceService = DependencyService.Get<ICurrentDeviceService>();
        public ISpecialDeviceInfo SpecialDeviceInfo = DependencyService.Get<ISpecialDeviceInfo>();
        public BaseViewModel()
        {
            SwitchDatabase();
        }
        public void SwitchDatabase()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                Repository = DependencyService.Get<FirebaseParentalControlRepo>();
            }
            else
            {
                Repository = DependencyService.Get<SQLiteParentalControlRepo>();
            }
        }
        public void SwitchToOfflineDB()
        {
            Repository = DependencyService.Get<SQLiteParentalControlRepo>();
        }
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
    }
}
