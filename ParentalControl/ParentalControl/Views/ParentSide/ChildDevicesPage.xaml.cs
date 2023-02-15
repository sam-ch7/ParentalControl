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
    public partial class ChildDevicesPage : ContentPage
    {
        ChildDevicesViewModel viewModel;
        public ChildDevicesPage()
        {
            InitializeComponent();
            BindingContext=viewModel = new ChildDevicesViewModel();
        }
        protected override void OnAppearing()
        {
            viewModel.OnAppearing();
        }
    }
}