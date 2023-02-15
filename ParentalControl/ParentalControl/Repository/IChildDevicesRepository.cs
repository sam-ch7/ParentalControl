using ParentalControl.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.Repository
{
    public interface IChildDevicesRepository
    {
        void AddChildDevice();
        Task<List<ChildDevice>> GetChildDevices();
        void OnLogout();
        void RemoveChildDevice();
    }
}
