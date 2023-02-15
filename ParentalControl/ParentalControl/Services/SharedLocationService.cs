using ParentalControl.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ParentalControl.Services
{
    public class SharedLocationService:BaseViewModel
    {
		readonly bool stopping = false;
		public SharedLocationService()
		{
		}

		public async Task Run(CancellationToken token)
		{
			await Task.Run(async () => {
				while (!stopping)
				{
					token.ThrowIfCancellationRequested();
					try
					{
						await Task.Delay(2000);

						var request = new GeolocationRequest(GeolocationAccuracy.High);
						var location = await Geolocation.GetLocationAsync(request);
						if (location != null)
						{
                            var message = new Location
                            {
                                Latitude = location.Latitude,
                                Longitude = location.Longitude
                            };

                            Device.BeginInvokeOnMainThread(async () =>
							{
								await Repository.LocationActivitiesRepository.UploadLocation(location);
								Console.WriteLine("---------------------------------------------------");
							});
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
						Device.BeginInvokeOnMainThread(() =>
						{
							//var errormessage = new LocationErrorMessage();
							//MessagingCenter.Send(errormessage, "LocationError");
						});
					}
				}
				return;
			}, token);
		}
	}
}
