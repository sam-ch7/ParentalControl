using ParentalControl.Models;
using ParentalControl.ViewModels.Base;
using ParentalControl.Views;
using ParentalControl.Views.ParentSide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ParentalControl.ViewModels
{
    public class ChildDevicesViewModel : BaseViewModel
    {
        public ObservableCollection<ChildDevice> ChildDevices { get; }
        public Command LoadChildDevicesCommand { get; }
        public Command<ChildDevice> ViewChildDeviceCommand { get; }
        public ChildDevicesViewModel()
        {
            ChildDevices = new ObservableCollection<ChildDevice>();
            LoadChildDevicesCommand = new Command(async () => await ExecLoadChildDevicesCommand());
            ViewChildDeviceCommand = new Command<ChildDevice>(async (ChildDevice childDevice) => await ExecViewCDCommand(childDevice));
        }
        internal void OnAppearing()
        {
            IsBusy = true;
        }
        async Task ExecViewCDCommand(ChildDevice childDevice)
        {
            SelectedChildDevice.ID = childDevice.ID;
            SelectedChildDevice.Name = childDevice.Name;
            SelectedChildDevice.Platform = childDevice.Platform;
            SelectedChildDevice.Manufacturer = childDevice.Manufacturer;
            SelectedChildDevice.VersionString = childDevice.VersionString;
            SelectedChildDevice.Model = childDevice.Model;
            SelectedChildDevice.IsAccessibilityServiceEnabled = childDevice.IsAccessibilityServiceEnabled;
            SelectedChildDevice.IsLoggedIn = childDevice.IsLoggedIn;
            await Shell.Current.GoToAsync(nameof(DeviceActivitiesOptionsPage));
        }
        async Task ExecLoadChildDevicesCommand()
        {
            IsBusy = true;
            base.SwitchDatabase();
            try
            {
                ChildDevices.Clear();
                var CDs = await Repository.ChildDevicesRepository.GetChildDevices();
                foreach (var cd in CDs)
                {
                    ChildDevices.Add(cd);
                }

                //base.SwitchToOfflineDB();
                //Repository.ChildDevicesRepository.AddChildDevice();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
