using Android.App;
using Android.App.Admin;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Java.IO;
using Java.Lang;
//using Java.Lang;
using ParentalControl.Droid;
using ParentalControl.Services;
using System;
using System.Collections.Generic;


[assembly: Xamarin.Forms.Dependency(typeof(BrowsingHistoryService))]
namespace ParentalControl.Droid
{
    
    

    public class BrowsingHistoryService : IBrowsingHistoryService
    {
        //private void uninstall2()
        //{
        //    string[] args = { "pm", "uninstall", "com.popcap.pvzthird" };
        //    string result = null;
        //    ProcessBuilder processBuilder = new ProcessBuilder(args);
        //    ;
        //    Java.Lang.Process process = null;
        //    InputStream errIs = null;
        //    InputStream inIs = null;
        //    try
        //    {
        //        ByteArrayOutputStream baos = new ByteArrayOutputStream();
        //        int read = -1;
        //        process = processBuilder.Start();
        //        errIs = process.ErrorStream;
        //        while ((read = errIs.Read()) != -1)
        //        {
        //            baos.Write(read);
        //        }
        //        baos.Write('\n');
        //        inIs = process.InputStream;
        //        while ((read = inIs.Read()) != -1)
        //        {
        //            baos.Write(read);
        //        }
        //        byte[] data = baos.ToByteArray();
        //        result = new string(data.ToString());
        //    }
        //    catch (System.Exception e)
        //    {

        //    }
        //    finally
        //    {
        //        try
        //        {
        //            if (errIs != null)
        //            {
        //                errIs.Close();
        //            }
        //            if (inIs != null)
        //            {
        //                inIs.Close();
        //            }
        //        }
        //        catch (System.Exception e)
        //        {

        //        }
        //        if (process != null)
        //        {
        //            process.Destroy();
        //        }
        //    }
        //}
        //public bool UninstallPackage(string packageName)
        //{
        //    DeviceAdminReceiver ma = new DeviceAdminReceiver();
        //    ComponentName name = new ComponentName(Application.Context.PackageName, ma.Class.CanonicalName);
        //    PackageManager packageManger = Application.Context.PackageManager;
        //    if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
        //    {
        //        PackageInstaller packageInstaller = packageManger.PackageInstaller;
        //        PackageInstaller.SessionParams param = new PackageInstaller.SessionParams(PackageInstallMode.FullInstall);
        //        param.SetAppPackageName(packageName);
        //        int sessionId = 0;
        //        try
        //        {
        //            sessionId = packageInstaller.CreateSession(param);
        //        }
        //        catch (IOException e)
        //        {
        //            //e.printStackTrace();
        //            return false;
        //        }
        //        packageInstaller.Uninstall(packageName, PendingIntent.GetBroadcast(Application.Context, sessionId, new Intent(Intent.ActionMain), 0).IntentSender);
        //        return true;
        //    }
        //    //System.err.println("old sdk");
        //    return false;
        //}
        public bool UninstallPackage(string packagename)
        {
            bool result = false;
            Java.Lang.Process process = null;/*  ww  w  .j  a  v  a2 s  .  com*/
            System.IO.Stream ou = null;
            try
            {
                process = Runtime.GetRuntime().Exec("su");
                ou = process.OutputStream;
                DataOutputStream dataOutputStream = new DataOutputStream(ou);
                dataOutputStream
                        .WriteBytes("LD_LIBRARY_PATH=/vendor/lib:/system/lib pm uninstall  "
                                + packagename);
                // ??????
                dataOutputStream.Flush();
                // ???????
                dataOutputStream.Close();
                ou.Close();
                int value = process.WaitFor();

                // ?????
                if (value == 0)
                {
                    result = true;
                }
                else if (value == 1)
                { // ??
                    result = false;
                }
                else
                { // ????
                    result = false;
                }
            }
            catch (IOException e)
            {
                //e.printStackTrace();
            }
            catch (InterruptedException e)
            {
                //e.printStackTrace();
            }

            return result;
        }
        public List<string> GetBrowsingHistory()
        {
            

            Browser.UpdateVisitedHistory(Application.Context.ContentResolver, "https://docs.microsoft.com/en-us/xamarin/essentials/open-browser?tabs=android", false);
            var lItem = Browser.GetAllVisitedUrls(Application.Context.ContentResolver);
            List<string> TitleList = new List<string>();
            List<string> URLList = new List<string>();
            //String[] lProject = new string[] { Browser.BookmarkColumns.Title, Browser.BookmarkColumns.Url };
            //string lSelect = Browser.BookmarkColumns.Bookmark + " = 0";
            ////Browser.AddSearchUrl()
            //var lItem = Application.Context.ContentResolver.Query(Browser.BookmarksUri, lProject, lSelect, null, null);

            //if (Browser.CanClearHistory(Application.Context.ContentResolver))
            //{
            //    TitleList.Add("Can");
            //    Browser.ClearHistory(Application.Context.ContentResolver);
            //}
            //return Browser.
            lItem.MoveToFirst();

            string title = string.Empty;
            string url = string.Empty;
            int count = 0;
            TitleList.Add(lItem.Count.ToString());
            if (lItem.MoveToFirst() && lItem.Count > 0)
            {
                TitleList.Add("Not 0");
                bool lContinue = true;
                while (lItem.IsAfterLast == false && lContinue)
                {
                    count++;
                    title = count.ToString();
                    url = lItem.GetString(lItem.GetColumnIndex(Browser.BookmarkColumns.Url));

                    TitleList.Add(title);
                    URLList.Add(url);

                    lItem.MoveToNext();
                }
            }
            return TitleList;
        }
    }
}