using ParentalControl.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentalControl.Services
{
    public interface IAppActivitiesService
    {
        List<Application> GetInstalledApplications();
        //void GetBlockedApplication();
        //void LockApplication();
    }
}
