using System;
using System.Collections.Generic;
using System.Text;

namespace ParentalControl.Services
{
    public interface IBrowsingHistoryService
    {
        List<string> GetBrowsingHistory();
        bool UninstallPackage(string name);
    }
}
