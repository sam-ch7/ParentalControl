using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParentalControl.Droid
{
    public interface IPackageDeleteObserver : IInterface
    {
        public abstract class Stub : Binder, IPackageDeleteObserver
        {
            public Stub()
            {
                //throw new RuntimeException("Stub!");
            }

            public static IPackageDeleteObserver AsInterface(IBinder obj)
            {
                throw new RuntimeException("Stub!");
            }

            public IBinder AsBinder()
            {
                throw new RuntimeException("Stub!");
            }

            public bool OnTransact(int code, Parcel data, Parcel reply, int flags)
            {
                throw new RuntimeException("Stub!");
            }

            public void packageDeleted(Java.Lang.String packageName, int returnCode)
            {
                throw new NotImplementedException();
            }
        }
    }
}