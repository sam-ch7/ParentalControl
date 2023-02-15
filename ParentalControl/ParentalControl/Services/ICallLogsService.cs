using ParentalControl.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentalControl.Services
{
    public interface ICallLogsService
    {
        List<ChildCallLog> GetCallLogs();
    }
}
