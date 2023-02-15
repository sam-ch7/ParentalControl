using ParentalControl.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.Services
{
    public class CurrentDeviceService : ICurrentDeviceService
    {
        SQLiteAsyncConnection database;
        public CurrentDeviceService(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            //database.DropTableAsync<CurrentDevice>();
            database.CreateTableAsync<CurrentDevice>().Wait();
        }
        public async Task<bool> IsParentDevice()
        {
            try
            {
                var cd = await database.Table<CurrentDevice>().FirstOrDefaultAsync();
                return cd.IsParentDevice;
            }
            catch
            {
                return false;
            }
        }

        public async void SaveCurrentDevice(bool isparentdevice)
        {
            var cd = new CurrentDevice
            {
                ID = 0,
                IsParentDevice = isparentdevice
            };
            if (await database.Table<CurrentDevice>().FirstOrDefaultAsync() != null)
            {
                await database.UpdateAsync(cd);
            }
            else
            {
                await database.InsertAsync(cd);
            }
        }
    }
}
