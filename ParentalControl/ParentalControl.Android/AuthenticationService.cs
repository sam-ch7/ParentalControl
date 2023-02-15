using ParentalControl.Services;
using System.Threading.Tasks;
using Firebase.Auth;
using User = ParentalControl.Models.User;
using ParentalControl.Droid;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(AuthenticationService))]
namespace ParentalControl.Droid
{
    class AuthenticationService : IAuthenticationService
    {
        public bool IsSignedIn()
        {
            try
            {
                var user = FirebaseAuth.Instance.CurrentUser;
                if (user != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public async Task<string> SignInWithEmailAndPasswordAsync(string Email, string Password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(Email, Password);
                return (string)user.User.GetIdToken(false);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task<string> SignUpWithEmailAndPasswordAsync(User Model)
        {
            var user = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(Model.Email, Model.Password);
            var firebaseUserInfo = FirebaseAuth.Instance.CurrentUser;
            UserProfileChangeRequest profileUpdates = new UserProfileChangeRequest.Builder().SetDisplayName(Model.Username).Build();
            await firebaseUserInfo.UpdateProfileAsync(profileUpdates);
            return (string)user.User.GetIdToken(false);
        }
        public User GetCurrentUser()
        {
            try
            {
                var user = FirebaseAuth.Instance.CurrentUser;
                if (user == null)
                {
                    return null;
                }
                return new User { ID = user.Uid, Username = user.DisplayName, Email = user.Email };
            }
            catch
            {
                return null;
            }        }
        public FirebaseUser GetFirebaseUserID()
        {
            return FirebaseAuth.Instance.CurrentUser;
        }
        public void SignOut()
        {
            FirebaseAuth.Instance.SignOut();
        }

        public async Task<string> GetFirebaseAuthToken()
        {
            return FirebaseAuth.Instance.CurrentUser.GetIdToken(false).ToString();
        }
    }
}