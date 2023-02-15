using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Java.Util;
using ParentalControl.Droid;
using ParentalControl.Models;
using ParentalControl.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(CallLogsService))]
namespace ParentalControl.Droid
{
    public class CallLogsService : ICallLogsService
    {
        //public ChildCallLog GetRecentCallLog()
        //{
        //    string queryFilter = String.Format("{0}={1}", CallLog.Calls.Type, (int)CallType.Outgoing);
        //    string querySorter = String.Format("{0} desc ", CallLog.Calls.Date);
        //    Android.Database.ICursor queryData = Android.App.Application.Context.ContentResolver.Query(CallLog.Calls.ContentUri, null, queryFilter, null, querySorter);
        //    queryData.GetString(queryData.GetColumnIndex(CallLog.Calls.GetLastOutgoingCall(Android.App.Application.Context)));
        //    queryData.GetString(queryData.GetColumnIndex(CallLog.Calls.New));
        //    return new ChildCallLog
        //    {
        //        CallNumber = 
        //    }
        //}
        public List<ChildCallLog> GetCallLogs()
        {
            int i = 0;
            List<ChildCallLog> calllogsBuffer = new List<ChildCallLog>();
            calllogsBuffer.Clear();
            Android.Database.ICursor managedCursor = Android.App.Application.Context.ContentResolver.Query(CallLog.Calls.ContentUri,
                    null, null, null, null);
            int number = managedCursor.GetColumnIndex(CallLog.Calls.Number);
            int type = managedCursor.GetColumnIndex(CallLog.Calls.Type);
            int date = managedCursor.GetColumnIndex(CallLog.Calls.Date);
            int duration = managedCursor.GetColumnIndex(CallLog.Calls.Duration);
            int name = managedCursor.GetColumnIndex(CallLog.Calls.CachedName);
            while (managedCursor.MoveToNext())
            {
                string CName = managedCursor.GetString(name);
                string phNumber = managedCursor.GetString(number);
                String callType = managedCursor.GetString(type);
                String callDate = managedCursor.GetString(date);
                Date callDayTime = new Date(long.Parse(callDate));
                String callDuration = managedCursor.GetString(duration);
                String dir = null;
                int dircode = Convert.ToInt32(callType);
                switch (dircode)
                {
                    case (int)CallType.Outgoing:
                        dir = "OUTGOING";
                        break;
                    case (int)CallType.Incoming:
                        dir = "INCOMING";
                        break;
                    case (int)CallType.Missed:
                        dir = "MISSED";
                        break;
                }
                calllogsBuffer.Add(new ChildCallLog { CallNumber = phNumber, CallType = dir, CallDate = callDayTime.ToString(), Duration = callDuration,ContactName=CName });
                
            }
            calllogsBuffer.Reverse();
            managedCursor.Close();
            return calllogsBuffer.GetRange(0,30);
        }
        
    }
}