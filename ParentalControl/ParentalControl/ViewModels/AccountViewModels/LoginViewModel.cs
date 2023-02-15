using ParentalControl.Models;
using ParentalControl.ViewModels.Base;
using ParentalControl.Views;
using Xamarin.Forms;

namespace ParentalControl.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command SignUpPageCommand { get; }
        public User Model { get; set; } = new User();
        //public string Email
        //{
        //    get => Model.Email;
        //    set => SetProperty(ref Model.Email, value);
        //}
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            SignUpPageCommand = new Command(OnSignUpPageClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            string auth= await AuthenticationService.SignInWithEmailAndPasswordAsync(Model.Email,Model.Password);
            if (auth != null)
            {
                CurrentDeviceService.SaveCurrentDevice(true);
                await Shell.Current.GoToAsync(nameof(SelectDevicePage));
            }
            else
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Unable To Login", "Check Email and Password", "Cancel");
            }
        }
        private async void OnSignUpPageClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync(nameof(SignUpPage));
        }
    }
}
