using Firebase.Database;
using Firebase.Database.Offline;
using ParentalControl.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentalControl.Repository.FirebaseRepo
{
    public class FirebaseParentalControlRepo : IParentalControlRepository
    {
        FirebaseClient _firebase;
        public FirebaseParentalControlRepo(string ConnectionUrl,IAuthenticationService authService)
        {
            //FirebaseOptions options = new FirebaseOptions()
            //{
            //    OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s),
            //    AuthTokenAsyncFactory = async () => await authService.GetFirebaseAuthToken()
            //};
            _firebase = new FirebaseClient(ConnectionUrl);
        }
        public IOnlineActivitiesRepository OnlineActivitiesRepository => new FirebaseOnlineActRepo(_firebase);
        public IApplicationActivitiesRepository ApplicationActivitiesRepository => new FirebaseAppActRepo(_firebase);
        public IChildDevicesRepository ChildDevicesRepository => new FirebaseChildDevicesRepo(_firebase);

        public ILocationActivitiesRepository LocationActivitiesRepository => new FirebaseLocationActRepo(_firebase);

        public ICommunicationActivitiesRepository CommunicationActivitiesRepository => new FirebaseCommActRepo(_firebase);
        //public IUserRepository User => new FirebaseUserRepo(_firebase);
    }
}
