using ParentalControl.Models;
using ParentalControl.Services;
using ParentalControl.ValidationRules;
using ParentalControl.ViewModels.Base;
using ParentalControl.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ParentalControl.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        public User Model { get; set; } = new User();
        public Command SignUpCommand { get; }

        private bool _EmailValid;
        public bool EmailValid
        {
            get => _EmailValid;
            set => SetProperty(ref _EmailValid, value);
        }
        public SignUpViewModel()
        {
            SignUpCommand = new Command(OnSignUpClicked);
        }
        public async void OnSignUpClicked()
        {
            if (!EmailValid)
            {
                Debug.Write("error");
                return;
            }
            Model.Username = Model.Firstname + Model.Lastname;
            await AuthenticationService.SignUpWithEmailAndPasswordAsync(Model);
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
