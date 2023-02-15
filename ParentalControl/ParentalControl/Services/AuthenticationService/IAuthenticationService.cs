//using Firebase.Auth;
using ParentalControl.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.Services
{
    public interface IAuthenticationService
    {
        Task<string> SignUpWithEmailAndPasswordAsync(User model);
        Task<string> SignInWithEmailAndPasswordAsync(string Email, string Password);
        //Task<User> GetLoggedInProfile();
        bool IsSignedIn();
        User GetCurrentUser();
        void SignOut();
        Task<string> GetFirebaseAuthToken();
    }
}
