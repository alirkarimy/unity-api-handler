using System;
using Alec.Api;
using Alec.Core;
namespace Alec.Api
{
    public class AlecListener
    {
      
        /// <summary>
        /// Occures when WebCore initializations input params receive correctly
        /// </summary>
        public static Action WebCoreInitialized;
        public static Action<string> WebCoreInitializeFailed;

        public static Action<string> OnUserRegisterRecived;
        public static Action<ResponseStatus, string> OnUserRegisterFailed;

        public static Action<LoginModel> OnLoginRecived;
        public static Action<ResponseStatus, string> OnLoginFailed;

        public static Action<UserDataModel> OnUserProfileRecived;
        public static Action<ResponseStatus, string> OnUserProfileFailed;

        public static Action<UserDataModel> OnSignup;
        public static Action<ResponseStatus, string> OnSignupFailed;

        public static Action<string> OnUpdatePlayerRecived;
        public static Action<ResponseStatus, string> OnUpdatePlayerFailed;


        public static Action OnTokenExpired;

    }
}
