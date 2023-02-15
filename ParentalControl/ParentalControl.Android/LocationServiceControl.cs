using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ParentalControl.Droid;
using ParentalControl.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
[assembly: Xamarin.Forms.Dependency(typeof(ServicesControl))]
namespace ParentalControl.Droid
{
    public class ServicesControl:IServicesControl
    {
        
        public void StopLocationService()
        {
            Intent intent = new Intent(Application.Context, typeof(LocationService));
            Android.App.Application.Context.StopService(intent);
        }
    }
}