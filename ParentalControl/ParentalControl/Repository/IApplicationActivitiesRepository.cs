using ParentalControl.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.Repository
{
    public interface IApplicationActivitiesRepository
    {
        void UploadInstalledApplications(List<Application> applications = null);
        Task<List<Application>> GetInstalledApplicationsList(string deviceID);
        Task<bool> AddApplicationToBlockList(Application app);
        Task<List<Application>> GetBlockedApplications();
        void RemoveAppFromBlockList(Application app);
    }
}
