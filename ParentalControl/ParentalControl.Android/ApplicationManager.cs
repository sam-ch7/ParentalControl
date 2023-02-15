using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Java.Interop;
using Java.Lang;
using Java.Lang.Reflect;
using System;

namespace ParentalControl.Droid
{
    public class PackageDeleteObserver : IPackageDeleteObserver.Stub
    {
       
        public void packageDeleted(Java.Lang.String packageName, int returnCode)
        {
            /*if (onInstalledPackaged != null) {
                onInstalledPackaged.packageInstalled(packageName, returnCode);
            }*/
        }

        
    }
    public class ApplicationManager
    {
        //private PackageInstallObserver observer;
        private PackageDeleteObserver observerdelete;
        private PackageManager pm;
        private Method method;
        private Method uninstallmethod;
        public ApplicationManager(Context context)
        {
            observerdelete = new PackageDeleteObserver();
            pm = context.PackageManager;

            //Class[] types = new Class[] { Class.FromType(typeof(Uri)), Class.FromType(typeof(IPackageInstallObserver)), int.class, String.class};
            Class[] uninstalltypes = new Class[] { Class.FromType(typeof(Java.Lang.String)), Class.FromType(typeof(IPackageDeleteObserver)), Class.FromType(typeof(int))};

            //method = pm.getClass().getMethod("installPackage", types);
            uninstallmethod = pm.Class.GetMethod("deletePackage", uninstalltypes);
        }
        public void uninstallPackage(string packagename) 
        {
            uninstallmethod.Invoke(pm, new Java.Lang.Object[] { packagename, observerdelete, 0 });
        }
    }
}


 