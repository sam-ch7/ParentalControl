using ParentalControl.Views;
using ParentalControl.Views.ParentSide;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ParentalControl.ViewModels
{
    public class DAOPViewModel
    {
        public Command AppsCommand { get; }
        public Command WebCommand { get; }
        public Command ComCommand { get; }
        public Command LocCommand { get; }
        public Command CallogsCommand { get; }
        public Command ContactsCommand { get; }
        public DAOPViewModel()
        {
            AppsCommand = new Command(async () => await ExecAppsCommand());
            WebCommand = new Command(async () => await ExecWebCommand());
            ComCommand = new Command(async () => await ExecComCommand());
            LocCommand = new Command(async () => await ExecLocCommand());
            CallogsCommand = new Command(async () => await ExecCallLogsCommand());
        }
        public async Task ExecCallLogsCommand()
        {
            Console.WriteLine("lll");
            await Shell.Current.GoToAsync(nameof(CallLogsListPage));
        }
        public async Task ExecContactsCommand()
        {
            Console.WriteLine("lll");
            await Shell.Current.GoToAsync(nameof(InstalledAppsPage));
        }
        public async Task ExecAppsCommand()
        {
            Console.WriteLine("lll");
            await Shell.Current.GoToAsync(nameof(InstalledAppsPage));
        }
        public async Task ExecWebCommand()
        {
            Console.WriteLine("lll");
            await Shell.Current.GoToAsync(nameof(BlockedWebsitesPage));
        }
        public async Task ExecComCommand()
        {
            Console.WriteLine("lll");
            await Shell.Current.GoToAsync(nameof(CommunicationActivitiesOptionsPage));
        }
        public async Task ExecLocCommand()
        {
            Console.WriteLine("lll");
            await Shell.Current.GoToAsync(nameof(MapView));
        }
        
    }
}
