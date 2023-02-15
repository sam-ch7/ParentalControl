using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ParentalControl.Droid;
using ParentalControl.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Android.Provider.Settings;
[assembly: Xamarin.Forms.Dependency(typeof(AndroidDeviceInfo))]
namespace ParentalControl.Droid
{
    public class AndroidDeviceInfo : ISpecialDeviceInfo
    {
        public string GetDeviceID()
        {
            var context = Application.Context;
            string id = Secure.GetString(context.ContentResolver, Secure.AndroidId);
            return id;
        }
        public bool IsAccessibilityServiceEnabled()
        {
            var context = Application.Context;
            int accessEnabled = 0;
            try
            {
                accessEnabled = Secure.GetInt(context.ContentResolver, Secure.AccessibilityEnabled);
            }
            catch
            {
                return false;//e.PrintStackTrace();
            }
            if (accessEnabled == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}