using System;
using Alec.Api;
namespace Alec.Api
{
    public class AlecListener
    {
        /// <summary>
        /// Occures when WebCore initializations input params receive correctly
        /// </summary>
        public static Action WebCoreInitialized;
        public static Action<string> WebCoreInitializeFailed;

        public static Action<AppConfigModel> OnAppConfigReceived;
        public static Action<ResponseStatus, string> OnAppConfigFailed;

        public static Action<string> OnUserRegisterRecived;
        public static Action<ResponseStatus, string> OnUserRegisterFailed;

        public static Action<LoginModel> OnLoginRecived;
        public static Action<ResponseStatus, string> OnLoginFailed;

        public static Action<GetUserProfileApiModel> OnGetUserProfileRecived;
        public static Action<ResponseStatus, string> OnGetUserProfileFailed;

        public static Action<string> OnUpdatePlayerRecived;
        public static Action<ResponseStatus, string>  OnUpdatePlayerFailed;

    public static Action OnTokenExpired;

    }
}
