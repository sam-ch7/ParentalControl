using Android;
using Android.AccessibilityServices;
using Android.App;
using Android.App.Usage;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Test.Mock;
using Android.Views;
using Android.Views.Accessibility;
using Android.Widget;
using AndroidX.AppCompat.App;
using Java.Lang;
using Java.Util;
using ParentalControl.Droid;
using ParentalControl.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using static Android.Icu.Text.Edits;

[assembly: Xamarin.Forms.Dependency(typeof(AppBlockService))]
namespace ParentalControl.Droid
{
    //To run this service, start it in MainActivity or anywhere with an intent and enable app usage in device
    [Service(Label = "AppBlockService",Permission = Manifest.Permission.ForegroundService)]
    public class AppBlockService : Service, IAppActivitiesService
    {
        BackgroundWorker worker;
        public List<Models.Application> GetInstalledApplications()
        {
            List<Models.Application> applications = new List<Models.Application>();
            PackageManager packageManager = Application.Context.PackageManager;
            List<ApplicationInfo> packages = packageManager.GetInstalledApplications(0).ToList();
            foreach(var app in packages)
            {
                if ((app.Flags & ApplicationInfoFlags.System) != 0)
                {

                }
                else
                {
                    applications.Add(new Models.Application
                    {
                        Name = app.LoadLabel(packageManager),
                        PackageName = app.PackageName,
                        //Logo = app.LoadLogo(packageManager),
                        IsBlocked=false
                    });
                }
                    
            }
            return applications;
        }
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
        public override void OnCreate()
        {
            Console.WriteLine("");
        }
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerAsync();
            return base.OnStartCommand(intent, flags, startId);
        }
        public void StopService()
        {
            var intent = new Intent(Application.Context, typeof(AppBlockService));
            Application.Context.StopService(intent);
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            CheckRunningApps();
        }

        public void LockApplication()
        {
        }
        public void CheckRunningApps()
        {
            while (true)
            {
                Thread.Sleep(1000);
                UsageStatsManager mUsageStatsManager = (UsageStatsManager)Application.Context.GetSystemService("usagestats");
                long time = JavaSystem.CurrentTimeMillis();
                long beginTime = time - 1000 * 10;
                string mPackageName = "";
                // We get usage stats for the last 10 seconds
                List<UsageStats> stats = mUsageStatsManager.QueryUsageStats(UsageStatsInterval.Daily, beginTime, time).ToList<UsageStats>();
                // Sort the stats by the last time used
                if (stats.Count > 0)
                {
                    SortedDictionary<long, UsageStats> mySortedMap = new SortedDictionary<long, UsageStats>();
                    foreach (UsageStats usageStats in stats)
                    {
                        try
                        {
                            if (usageStats.PackageName.Contains("mediatek"))
                            {

                            }
                            else
                            {
                                mySortedMap.Add(usageStats.LastTimeUsed, usageStats);
                            }
                            
                        }
                        catch
                        {

                        }
                    }
                    if (mySortedMap.Count > 0)
                    {
                        mPackageName = mySortedMap.Last().Value.PackageName;
                    }
                    Console.WriteLine(mPackageName);
                    if (mPackageName=="com.facebook.katana")
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            Console.WriteLine("------");
                            Intent intent = new Intent(Settings.ActionAccessibilitySettings);
                            intent.SetFlags(Android.Content.ActivityFlags.ClearTop | Android.Content.ActivityFlags.ClearTask | Android.Content.ActivityFlags.NewTask | Android.Content.ActivityFlags.SingleTop);
                            // request permission via start activity for result
                            StartActivity(intent);
                        });
                        
                    }
                }
                else
                {
                    
                }
                
            }

        }

        public void GetBlockedApplication()
        {
            throw new NotImplementedException();
        }
    }
}