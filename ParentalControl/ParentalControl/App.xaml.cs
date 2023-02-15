using ParentalControl.Repository;
using ParentalControl.Repository.FirebaseRepo;
using ParentalControl.Repository.SQLiteRepo;
using ParentalControl.Services;
using System;
using System.IO;
using Xamarin.Forms;

namespace ParentalControl
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            Sharpnado.MaterialFrame.Initializer.Initialize(false,true);
            DependencyService.Register<MockDataStore>();
            var FBRepository = new FirebaseParentalControlRepo("https://parentalcontrol-5f1a4-default-rtdb.firebaseio.com/", DependencyService.Get<IAuthenticationService>());
            DependencyService.RegisterSingleton(FBRepository);
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ParentalControlDB.db3");
            var SQLRepository = new SQLiteParentalControlRepo(dbPath);
            DependencyService.RegisterSingleton(SQLRepository);
            var CD = new CurrentDeviceService(dbPath);
            DependencyService.RegisterSingleton<ICurrentDeviceService>(CD);
            MainPage = new AppShell();

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
