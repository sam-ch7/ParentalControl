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
    public partial class CommunicationActivitiesOptionsPage : ContentPage
    {
        public CommunicationActivitiesOptionsPage()
        {
            InitializeComponent();
            BindingContext = new DAOPViewModel();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
}