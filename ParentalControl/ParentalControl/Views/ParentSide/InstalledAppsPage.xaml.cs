using ParentalControl.ViewModels.ApplicationActivitiesViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParentalControl.Views.ParentSide
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InstalledAppsPage : ContentPage
    {
        InstalledApplicationsViewModel applicationsViewmodel;
        public InstalledAppsPage()
        {
            InitializeComponent();
            BindingContext = applicationsViewmodel = new InstalledApplicationsViewModel();
        }
        protected override void OnAppearing()
        {
            applicationsViewmodel.OnAppearing();
        }
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void BlockBt_Clicked(object sender, EventArgs e)
        {
            var bt = sender as Button;
            var App = bt.BindingContext as Models.Application;
            applicationsViewmodel.ExecBlockAppCommand(App);
        }
    }
}
