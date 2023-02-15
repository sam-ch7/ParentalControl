//using Firebase.Auth;
//using ParentalControl.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using User = ParentalControl.Models.User;

//namespace ParentalControl.Services.FirebaseServices
//{
//    public class AuthenticationService : IAuthenticationService
//    {
//        string WebAPIKey = "AIzaSyDKuSb1wENqQ7qkn_bFaSnLcgluMKnkNLw";
//        FirebaseAuthProvider _FBAP;
//        FirebaseAuthLink _FBAL;
//        public AuthenticationService()
//        {
//            _FBAP = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
//        }

//        public async Task<Firebase.Auth.User> GetLoggedInProfile()
//        {
//            return await _FBAP.GetUserAsync(_FBAL);
//        }

//        public async Task<FirebaseAuthLink> SignInWithEmailAndPasswordAsync(User Model)
//        {
//            try
//            {
//                var auth = await _FBAP.SignInWithEmailAndPasswordAsync(Model.Email, Model.Password);
//                _FBAL = auth;
//                return auth;
//            }
//            catch(Exception e)
//            {
//                Console.WriteLine(e.Message);
//                return null;
//            }
//        }

//        public async Task<string> SignUpWithEmailAndPasswordAsync(User Model)
//        {
//            var auth = await _FBAP.CreateUserWithEmailAndPasswordAsync(Model.Email, Model.Password, Model.DisplayName, Model.IsEmailVerified);
//            await _FBAP.SendEmailVerificationAsync(auth.FirebaseToken);
//            return auth.FirebaseToken;
//        }
        
//    }
//}
