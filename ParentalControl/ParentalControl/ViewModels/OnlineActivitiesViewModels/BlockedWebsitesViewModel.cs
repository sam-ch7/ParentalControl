using ParentalControl.Models;
using ParentalControl.ViewModels.Base;
using ParentalControl.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ParentalControl.ViewModels.OnlineActivitiesViewModels
{
    public class BlockedWebsitesViewModel:BaseViewModel
    {
        private Website _selectedWebsite;

        public ObservableCollection<Website> BlockedWebsites { get; }
        public Command LoadWebsitesCommand { get; }
        public Command AddWebsiteCommand { get; }
        public Command RemoveWebsiteCommand { get; }
        internal void OnAppearing()
        {
            IsBusy = true;
        }

        public Command<Item> WebsiteTapped { get; }
        public BlockedWebsitesViewModel()
        {
            Title = "Blocked Websites";
            BlockedWebsites = new ObservableCollection<Website>();
            LoadWebsitesCommand = new Command(async () => await ExecuteLoadWebsitesCommand());

            //WebsiteTapped = new Command<Item>(OnWebsiteSelected);

            AddWebsiteCommand = new Command(OnAddWebsite);
            RemoveWebsiteCommand = new Command<Website>(OnRemoveWebsite);
            //OnAddWebsite();
        }

        private async void OnAddWebsite(/*object obj*/)
        {
            await Shell.Current.GoToAsync(nameof(AddBlockedWebsite));
        }

        private async void OnRemoveWebsite(Website obj)
        {
            Repository.OnlineActivitiesRepository.RemoveFromBlocklist(obj.URL);
            await ExecuteLoadWebsitesCommand();
        }

        private async Task ExecuteLoadWebsitesCommand()
        {
            IsBusy = true;
            try
            {
                BlockedWebsites.Clear();
                var websites = await Repository.OnlineActivitiesRepository.GetBlockedWebsites(SelectedChildDevice.ID);
                foreach (var website in websites)
                {
                    if (BlockedWebsites.Where(a => a.URL == website.URL).Count() > 0)
                    {
                    }
                    else
                    {
                        BlockedWebsites.Add(website);
                    }
                    //BlockedWebsites.Add(website);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
