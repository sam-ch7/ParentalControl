using ParentalControl.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.Repository
{
    public interface IOnlineActivitiesRepository
    {
        Task<List<Website>> GetBlockedWebsites(string DeviceID);
        Task<bool> AddBlockedWebsite(Website website);
        void RemoveFromBlocklist(string URL);
    }
}
