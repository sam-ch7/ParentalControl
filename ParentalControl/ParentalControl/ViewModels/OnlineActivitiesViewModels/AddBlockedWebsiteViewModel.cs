using ParentalControl.Models;
using ParentalControl.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ParentalControl.ViewModels.OnlineActivitiesViewModels
{
    public class AddBlockedWebsiteViewModel:BaseViewModel
    {
        private string name;
        private string url;

        public AddBlockedWebsiteViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(name)
                && !String.IsNullOrWhiteSpace(url);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string URL
        {
            get => url;
            set => SetProperty(ref url, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Website newBlockedSite = new Website()
            {
                Name = Name,
                URL = URL
            };
            await Repository.OnlineActivitiesRepository.AddBlockedWebsite(newBlockedSite);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
