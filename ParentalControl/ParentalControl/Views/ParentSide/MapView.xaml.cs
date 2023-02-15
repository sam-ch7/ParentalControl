using ParentalControl.Repository;
using ParentalControl.Repository.FirebaseRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ParentalControl.Views.ParentSide
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapView : ContentPage
    {
        private Pin pinRoute1 = new Pin
        {
            Label = "Child"
        };
        public IParentalControlRepository Repository = DependencyService.Get<FirebaseParentalControlRepo>();
        bool TimerRun;
        public MapView()
        {
            InitializeComponent();
            //var loc = Repository.LocationActivitiesRepository.GetLocation();
            //pinRoute1.Position = new Position(loc.Latitude, loc.Longitude);

            //ChildMap.MoveToRegion(MapSpan.FromCenterAndRadius(pinRoute1.Position, Xamarin.Forms.Maps.Distance.FromKilometers(1)));
            //ChildMap.Pins.Add(pinRoute1);
            var loc = Repository.LocationActivitiesRepository.GetLocation();
            pinRoute1.Position = new Position(loc.Latitude, loc.Longitude);
            //i=i+ 0.00009999999999;
            ChildMap.MoveToRegion(MapSpan.FromCenterAndRadius(pinRoute1.Position, Xamarin.Forms.Maps.Distance.FromKilometers(1)));
            //ChildMap.Pins.Add(pinRoute1);
            double i = 0;
            TimerRun = true;
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                try
                {
                    ChildMap.Pins.Remove(pinRoute1);
                }
                catch { }
                loc = Repository.LocationActivitiesRepository.GetLocation();
                pinRoute1.Position = new Position(loc.Latitude+i, loc.Longitude+i);
                //i=i+ 0.00009999999999;
                //ChildMap.MoveToRegion(MapSpan.FromCenterAndRadius(pinRoute1.Position, Xamarin.Forms.Maps.Distance.FromKilometers(1)));
                ChildMap.Pins.Add(pinRoute1);
                Console.WriteLine("Timer runinig----------------------------------");
                return TimerRun;
            });
        }
        protected override void OnDisappearing()
        {
            TimerRun = false;
        }
    }
}