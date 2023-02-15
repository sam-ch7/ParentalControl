using Android;
using Android.AccessibilityServices;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Views.Accessibility;
using Java.Util;
using ParentalControl.Repository;
using ParentalControl.Repository.FirebaseRepo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace ParentalControl.Droid
{
    [Service(Label = "ParentalControl", Permission = Manifest.Permission.BindAccessibilityService)]
    [IntentFilter(new[] { "android.accessibilityservice.AccessibilityService" })]
    [MetaData("android.accessibilityservice", Resource = "@drawable/accessibility_service_config")]
    public class WebsiteBlockService : AccessibilityService
    {
        public IParentalControlRepository Repository => DependencyService.Get<FirebaseParentalControlRepo>();
        BackgroundWorker worker;
        protected override void OnServiceConnected()
        {
            Repository.ChildDevicesRepository.AddChildDevice();
        }
        public override void OnDestroy()
        {
            Repository.ChildDevicesRepository.AddChildDevice();
        }
        private static string[] PackageNames()
        {
            List<string> packageNames = new List<string>();
            foreach (var config in GetSupportedBrowsers())
            {
                packageNames.Add(config.packageName);
            }
            return packageNames.ToArray();
        }
        public static List<SupportedBrowserConfig> GetSupportedBrowsers()
        {
            List<SupportedBrowserConfig> browsers = new List<SupportedBrowserConfig>();
            browsers.Add(new SupportedBrowserConfig("com.android.chrome", "com.android.chrome:id/url_bar"));
            browsers.Add(new SupportedBrowserConfig("org.mozilla.firefox", "org.mozilla.firefox:id/url_bar_title"));
            browsers.Add(new SupportedBrowserConfig("com.opera.browser", "com.opera.browser:id/url_field"));

            return browsers;
        }
        private string CaptureUrl(AccessibilityNodeInfo info, SupportedBrowserConfig config)
        {
            List<AccessibilityNodeInfo> nodes = info.FindAccessibilityNodeInfosByViewId(config.addressBarId).ToList();
            if (nodes == null || nodes.Count <= 0)
            {
                return null;
            }

            AccessibilityNodeInfo addressBarNodeInfo = nodes.ElementAt(0);
            string url = null;
            if (addressBarNodeInfo.Text != null)
            {
                url = addressBarNodeInfo.Text.ToString();
            }
            addressBarNodeInfo.Recycle();
            return url;
        }

        public override async void OnAccessibilityEvent(AccessibilityEvent even)
        {
            AccessibilityNodeInfo parentNodeInfo = even.Source;
            if (parentNodeInfo == null)
            {
                return;
            }
            string packageName = even.PackageName.ToString();
            if (packageName.Contains("parentalcontrol")
                || packageName.Contains("systemui") 
                || packageName.Contains("inputmethod"))
            {
                return;
            }
            else
            {
                try
                {
                    foreach (var app in await Repository.ApplicationActivitiesRepository.GetBlockedApplications())
                    {
                        if (packageName == app.PackageName)
                        {
                            PerformRedirect("", "");
                        }
                    }
                }
                catch
                {

                }
            }
            SupportedBrowserConfig browserConfig = null;
            
            Console.WriteLine(packageName); 
            foreach (SupportedBrowserConfig supportedConfig in GetSupportedBrowsers())
            {
                if (supportedConfig.packageName.Equals(packageName))
                {
                    browserConfig = supportedConfig;
                    Console.WriteLine("Equals");
                }
            }
            if (browserConfig == null)
            {
                return;
            }

            string capturedUrl = CaptureUrl(parentNodeInfo, browserConfig);
            parentNodeInfo.Recycle();

            //we can't find a url. Browser either was updated or opened page without url text field
            if (capturedUrl == null)
            {
                return;
            }

            long eventTime = even.EventTime;
            string detectionId = packageName + ", and url " + capturedUrl;
            //noinspection ConstantConditions
            long lastRecordedTime = (long)(previousUrlDetections.ContainsKey(detectionId) ? previousUrlDetections.Get(detectionId) : 0);
            //some kind of redirect throttling
            //previousUrlDetections.Put(detectionId, eventTime);
            //Console.WriteLine("TTTTTTTTTTTTTT");
            //AnalyzeCapturedUrlAsync(capturedUrl, browserConfig.packageName);
            
            //if (eventTime - lastRecordedTime > 2000)
            //{
                Console.WriteLine("hhhh");
                previousUrlDetections.Put(detectionId, eventTime);
                AnalyzeCapturedUrlAsync(capturedUrl, browserConfig.packageName);
            //}
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            worker.Dispose();
            worker = new BackgroundWorker();
        }

        private HashMap previousUrlDetections = new HashMap();
        private async void AnalyzeCapturedUrlAsync(string capturedUrl, string browserPackage)
        {
            string redirectUrl = "https://Website-Blocked-By-Your-Parent";
            foreach(var url in await Repository.OnlineActivitiesRepository.GetBlockedWebsites(new AndroidDeviceInfo().GetDeviceID()))
            {
                if (capturedUrl.ToLower().Contains(url.URL.ToLower()))
                {
                    PerformRedirect(redirectUrl, browserPackage);
                }
            }
            
        }
        private void PerformRedirect(string redirectUrl, string browserPackage)
        {
            try
            {
                Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(redirectUrl));
                intent.SetPackage(browserPackage);
                intent.PutExtra(Browser.ExtraApplicationId, browserPackage);
                intent.AddFlags(Android.Content.ActivityFlags.ClearTop | Android.Content.ActivityFlags.ClearTask | Android.Content.ActivityFlags.NewTask | Android.Content.ActivityFlags.SingleTop);
                StartActivity(intent);
            }
            catch (ActivityNotFoundException e)
            {
                // the expected browser is not installed
                Intent intent = new Intent(Intent.ActionMain);
                intent.AddCategory(Intent.CategoryHome);
                intent.SetFlags(Android.Content.ActivityFlags.NewTask);
                // request permission via start activity for result
                StartActivity(intent);
            }
        }
        public override void OnInterrupt()
        {
            Repository.ChildDevicesRepository.AddChildDevice();
        }
    }
    public class SupportedBrowserConfig
    {
        public string packageName, addressBarId;
        public SupportedBrowserConfig(string packageName, string addressBarId)
        {
            this.packageName = packageName;
            this.addressBarId = addressBarId;
        }
    }
}