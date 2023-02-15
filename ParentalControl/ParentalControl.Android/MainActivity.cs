using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Provider;
using System.Collections.Generic;
using AndroidX.Core.App;
using Android;
using Android.Util;
using Google.Android.Material.Snackbar;
using Android.Views;
using Android.Widget;
using Android.Content;
using Firebase;
using Android.App.Admin;

namespace ParentalControl.Droid
{
    [Activity(Label = "ParentalControl", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //GetPermissionsAsync();
            if (!checkAccessibilityPermission())
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Permissions Needed");
                alert.SetMessage("The application need special permissions to continue");
                alert.SetPositiveButton("Request Permissions", (senderAlert, args) =>
                {
                    checkAccessibilityPermission();
                });

                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                });

                

                Dialog dialog = alert.Create();
                dialog.Show();
                Toast.MakeText(this, "Permission denied", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Accessibility Service", ToastLength.Short).Show();
            }
            //Toast.MakeText(this, "Permission granted", ToastLength.Short).Show();
            FirebaseApp.InitializeApp(Application.Context);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            Window.SetStatusBarColor(Android.Graphics.Color.Black);
            GetPermissionsAsync();
            Intent intent = new Intent(this, typeof(LocationService));
            StartService(intent);

            //var devicePolicyManager = (DevicePolicyManager)GetSystemService(Context.DevicePolicyService);
            //DeviceAdminService d = new DeviceAdminService();//ComponentName(this, d.Class);

            //Intent i = new Intent(DevicePolicyManager.ActionAddDeviceAdmin);// adds new device administrator
            //intent.PutExtra(DevicePolicyManager.ExtraDeviceAdmin, d.Class);//ComponentName of the administrator component.
            //intent.PutExtra(DevicePolicyManager.ExtraAddExplanation,
            //        "Disable app");//dditional explanation
            //StartActivityForResult(intent, 1);

            //Toast.MakeText(this, "Permission granted", ToastLength.Short).Show();

            //Intent intent = new Intent(Intent.ActionDelete);
            //intent.SetData(Android.Net.Uri.Parse("package:org.thunderdog.challegram"));
            //StartActivity(intent);

            //ApplicationManager ap = new ApplicationManager(this);
            //ap.uninstallPackage("com.cpuid.cpu_z");

        }
        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        //{
        //    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}
        public bool checkAccessibilityPermission()
        {
            int accessEnabled = 0;
            try
            {
                accessEnabled = Settings.Secure.GetInt(this.ContentResolver, Settings.Secure.AccessibilityEnabled);
            }
            catch (Settings.SettingNotFoundException e)
            {
                e.PrintStackTrace();
            }
            if (accessEnabled == 0)
            {
                // if not construct intent to request permission
                Intent intent = new Intent(Settings.ActionAccessibilitySettings);
                intent.SetFlags(Android.Content.ActivityFlags.NewTask);
                // request permission via start activity for result
                StartActivity(intent);
                return false;
            }
            else
            {
                return true;
            }
        }
        public List<string> mTitleList = new List<string>();
        public List<string> mURLList = new List<string>();
        View layout;
        string[] RequiredPermissions = new String[] { Manifest.Permission.ReadCallLog
                };
        public async void GetPermissionsAsync()
        {
            const string permission = Manifest.Permission.ReadCallLog;

            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
            {
                //TODO change the message to show the permissions name
                Toast.MakeText(this, "Call Logs permissions granted", ToastLength.Short).Show();
                return;
            }

            if (ShouldShowRequestPermissionRationale(permission))
            {
                //set alert for executing the task
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Permissions Needed");
                alert.SetMessage("The application need special permissions to continue");
                alert.SetPositiveButton("Request Permissions", (senderAlert, args) =>
                {
                    RequestPermissions(RequiredPermissions, 0);
                });

                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                });

                Dialog dialog = alert.Create();
                dialog.Show();


                return;
            }

            RequestPermissions(RequiredPermissions, 0);

        }
        private void getBrowserHistory()
        {

            String[] lProject = new string[] { Browser.BookmarkColumns.Title, Browser.BookmarkColumns.Url };
            string lSelect = Browser.BookmarkColumns.Bookmark + " = 0";

            var lItem = ContentResolver.Query(Browser.BookmarksUri, lProject, lSelect, null, null);
            lItem.MoveToFirst();

            string title = string.Empty;
            string url = string.Empty;

            if (lItem.MoveToFirst() && lItem.Count > 0)
            {
                bool lContinue = true;
                while (lItem.IsAfterLast == false && lContinue)
                {
                    title = lItem.GetString(lItem.GetColumnIndex(Browser.BookmarkColumns.Title));
                    url = lItem.GetString(lItem.GetColumnIndex(Browser.BookmarkColumns.Url));

                    mTitleList.Add(title);
                    mURLList.Add(url);

                    lItem.MoveToNext();
                }
            }


        }
    }
}