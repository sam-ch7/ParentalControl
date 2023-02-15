using ParentalControl.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.Repository
{
    public interface ICommunicationActivitiesRepository
    {
        Task<List<ChildCallLog>> GetCallLogs(string DeviceID);
        void UploadCallLogsAsync();
    }
}
