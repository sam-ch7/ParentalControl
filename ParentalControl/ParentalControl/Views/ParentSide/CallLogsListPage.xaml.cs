using ParentalControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParentalControl.Views.ParentSide
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CallLogsListPage : ContentPage
    {
        CallLogsViewModel ViewModel;
        public CallLogsListPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new CallLogsViewModel();
        }
        protected override void OnAppearing()
        {
            ViewModel.OnAppearing();
        }
    }
}