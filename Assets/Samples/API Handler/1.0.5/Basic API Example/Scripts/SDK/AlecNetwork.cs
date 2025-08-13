using Alec.Core;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Alec.Api;

namespace Alec.Api
{
    public class AlecNetwork
    {

        private static bool _isInitialized = false;
        public static bool IsInitialized { get { return _isInitialized; } }

        public static void Initialize(string baseUrl, string deviceUUID, string appVersion, MonoBehaviour mono)
        {

            if (IsInitialized)
            {
                return;
            }

            if (mono == null || DataUtils.IsNullOrEmpty(baseUrl, deviceUUID, appVersion))
            {
                AlecListener.WebCoreInitializeFailed?.Invoke("Not Valid Arguments To Intialize WebCore");
            }
            AppConfig.OS_NAME = "android";
            AppConfig.OS_VERSION = "-1";
            AppConfig.APP_VERSION = appVersion;
           
            WebCore.OnTokenExpired += AlecListener.OnTokenExpired;
            WebCore.Initialize(mono,baseUrl,new Dictionary<string, string> {

                { "Accept", "application/json" },
                { "Content-Type", "application/json" },
                { "device-uuid", deviceUUID },
                { "language", deviceUUID },
                { "appVersion", appVersion },
                { "firebase-token", deviceUUID }

            });

            AlecListener.WebCoreInitialized?.Invoke();
        }
      
        public static void UpdatePlayer(int playerId, int familyRelationShip, int grade, string firstName, string lastName, string birthDay, int gender, int avatarId, int appUsageLimitMinute)
        {
            UpdatePlayerApi api = new UpdatePlayerApi(playerId);
            api.JSONData = JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                {"family_relationship", familyRelationShip.ToString()},
                {"grade", grade.ToString()},
                {"first_name" , firstName},
                {"last_name" , lastName},
                {"birthday", birthDay},
                {"gender", gender.ToString()},
                {"avatar_id" , avatarId.ToString()},
                {"app_usage_limit_minute" , appUsageLimitMinute.ToString()}
            });
            WebCore.Put(api);
        }
        public static void SignupAsGuest(string name)
        {
            SignupApi signupApi = new SignupApi();
            signupApi.JSONData = DataUtils.ConvertToJSON(new Dictionary<string, string>
            {
                {"name", name} 
            });
            WebCore.Post< Response<UserDataModel>>(signupApi);

        }
        public static void GetUserProfile()
        {
            GetUserProfileApi api = new GetUserProfileApi();
            WebCore.Get< Response<UserDataModel>>(api);
        }
        public static void LoginWithPassword(string mobileNumber, string mobileNumberPrefix, string password,string clientToken,string clientSecret)
        {
            Login(new Dictionary<string, string>
            {
                {"mobile_number", mobileNumber},
                {"mobile_number_prefix", mobileNumberPrefix},
                {"grant_type", "password_grant"},
                {"password", password},
                {"current_version", AppConfig.APP_VERSION},
                {"client_id", clientToken},
                {"client_secret", clientSecret}

            });

        }
        public static void LoginWithOtp(string mobileNumber, string mobileNumberPrefix, string Otp, string clientToken, string clientSecret)
        {
            Login(new Dictionary<string, string>
            {
                {"mobile_number", mobileNumber},
                {"mobile_number_prefix", mobileNumberPrefix},
                {"grant_type", "otp_grant"},
                {"otp", Otp},
                {"current_version", AppConfig.APP_VERSION},
                {"client_id", clientToken},
                {"client_secret", clientSecret}
            });
        }
        public static void LoginWithToken(string mobileNumber, string mobileNumberPrefix, string refreshToken, string clientToken, string clientSecret)
        {
            Login(new Dictionary<string, string>
            {
                {"mobile_number", mobileNumber},
                {"mobile_number_prefix", mobileNumberPrefix},
                {"grant_type", "refresh_token"},
                {"refresh_token", refreshToken},
                {"current_version", AppConfig.APP_VERSION},
                {"client_id", clientToken},
                {"client_secret", clientSecret}
            });
        }
        public static void Login(Dictionary<string,string> bodyData)
        {
            LoginApi api = new LoginApi();
            api.FormData = bodyData;
            WebCore.Post<Response<LoginModel>>(api);
        }

     
       

    }
}