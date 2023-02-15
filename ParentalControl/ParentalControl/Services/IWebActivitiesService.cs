using System;
using System.Collections.Generic;
using System.Text;

namespace ParentalControl.Services
{
    public interface IWebActivitiesService
    {
        void AddWebsiteToBlockList(string URL);
        List<string> GetBlockList();
    }
}
