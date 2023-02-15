using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.Services
{
    public interface ICurrentDeviceService
    {
        void SaveCurrentDevice(bool isparentdevice);
        Task<bool> IsParentDevice();
    }
}
