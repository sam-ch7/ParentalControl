using ParentalControl.ViewModels.Base;
using ParentalControl.Views;
using ParentalControl.Views.ChildSide;
using ParentalControl.Views.ParentSide;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ParentalControl.ViewModels
{
    public class SelectDeviceViewModel:BaseViewModel
    {
        public Command ChildDeviceCommand { get; }
        public Command ParentDeviceCommand { get; }
        public SelectDeviceViewModel()
        {
            ChildDeviceCommand = new Command(OnChildDeviceSelected);
            ParentDeviceCommand = new Command(OnParentDeviceSelected);
            
        }
        private async void OnChildDeviceSelected()
        {
            CurrentDeviceService.SaveCurrentDevice(false);
            //await Shell.Current.GoToAsync(nameof(ConfigureDevicePage));
            Repository.ChildDevicesRepository.AddChildDevice();
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
        private async void OnParentDeviceSelected()
        {
            CurrentDeviceService.SaveCurrentDevice(true);
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
    }
}
