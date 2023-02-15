using ParentalControl.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.Repository.SQLiteRepo
{
    public class SQLiteAppActRepo : IApplicationActivitiesRepository
    {
        SQLiteAsyncConnection database;
        public SQLiteAppActRepo(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            //database.DropTableAsync<Application>();
            database.CreateTableAsync<Application>().Wait();
        }
        public Task<bool> AddApplicationToBlockList(Application app)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Application>> GetBlockedApplications()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Application>> GetInstalledApplicationsList(string ID)
        {
            return await database.Table<Application>().ToListAsync();
            //throw new NotImplementedException();
        }

        public void RemoveAppFromBlockList(Application app)
        {
            throw new NotImplementedException();
        }

        public async void UploadInstalledApplications(List<Application> applications=null)
        {
            foreach(var app in applications)
            {
                if (await database.Table<Application>()
                            .Where(i => i.PackageName == app.PackageName)
                            .CountAsync()>0)
                {
                    // Update an existing note.
                    await database.UpdateAsync(app);
                }
                else
                {
                    // Save a new note.
                    await database.InsertAsync(app);
                }
            }
        }
    }
}
