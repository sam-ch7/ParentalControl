using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ParentalControl.Repository.SQLiteRepo
{
    public class SQLiteParentalControlRepo : IParentalControlRepository
    {
        string _dbPath;
        public SQLiteParentalControlRepo(string dbPath)
        {
            _dbPath = dbPath;
        }
        public IOnlineActivitiesRepository OnlineActivitiesRepository => throw new NotImplementedException();
        public IApplicationActivitiesRepository ApplicationActivitiesRepository => new SQLiteAppActRepo(_dbPath);

        public IChildDevicesRepository ChildDevicesRepository => throw new NotImplementedException();

        public ILocationActivitiesRepository LocationActivitiesRepository => throw new NotImplementedException();

        public ICommunicationActivitiesRepository CommunicationActivitiesRepository => throw new NotImplementedException();
    }
}
