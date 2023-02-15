using Android;
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
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(LocationService))]
namespace ParentalControl.Droid
{
    [Service(Label = "LocationService", Permission = Manifest.Permission.ForegroundService)]
    public class LocationService : Service
    {
		CancellationTokenSource _cts;
		public const int SERVICE_RUNNING_NOTIFICATION_ID = 10001;

		public override IBinder OnBind(Intent intent)
		{
			return null;
		}

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			_cts = new CancellationTokenSource();

			//Notification notification = new NotificationHelper().GetServiceStartedNotification();
			//StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
			Console.WriteLine("---------------------------------------------------");
			Task.Run(() => {
				try
				{
					var locShared = new SharedLocationService();
					locShared.Run(_cts.Token).Wait();
				}
				catch (Android.OS.OperationCanceledException)
				{
				}
				finally
				{
					if (_cts.IsCancellationRequested)
					{
						//var message = new StopServiceMessage();
						Device.BeginInvokeOnMainThread(null
							//() => MessagingCenter.Send(message, "ServiceStopped")
						);
					}
				}
			}, _cts.Token);

			return StartCommandResult.Sticky;
		}

		public override void OnDestroy()
		{
			if (_cts != null)
			{
				_cts.Token.ThrowIfCancellationRequested();
				_cts.Cancel();
			}
			base.OnDestroy();
		}

        
    }
}