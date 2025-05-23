﻿using Alec.Core;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


namespace Alec.Api
{
    public class AlecNetwork
    {

        private static bool _isInitialized = false;
        public static bool IsInitialized { get { return _isInitialized; } }

        public static void Initialize(string baseUrl, string clinetId, string clientSecret, string deviceUUID, string storeName, string apkVersion, string osVersion, string osName, MonoBehaviour mono)
        {

            if (IsInitialized)
            {
                return;
            }

            if (mono == null || DataUtils.IsNullOrEmpty(baseUrl, clinetId, clientSecret, deviceUUID, storeName, apkVersion, osVersion, osName))
            {
                AlecListener.WebCoreInitializeFailed?.Invoke("Not Valid Arguments To Intialize WebCore");
            }
            AppConfig.OS_NAME = osName;
            AppConfig.OS_VERSION = osVersion;
            AppConfig.CURRENT_VERSION = apkVersion;
            UrlUtils.SetBaseUrl(baseUrl);
           
            WebCore.Initialize(mono,new Dictionary<string, string> {

                { "store-name", storeName },
                { "Accept", "application/json" },
                { "client-id", clinetId },
                { "client-secret", clientSecret },
                { "device-uuid", deviceUUID }

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
        public static void GetUserProfile()
        {
            GetUserProfileApi api = new GetUserProfileApi();
            WebCore.Get< Response<GetUserProfileApiModel>>(api);
        }
        public static void LoginWithPassword(string mobileNumber, string mobileNumberPrefix, string password,string clientToken,string clientSecret)
        {
            Login(new Dictionary<string, string>
            {
                {"mobile_number", mobileNumber},
                {"mobile_number_prefix", mobileNumberPrefix},
                {"grant_type", "password_grant"},
                {"password", password},
                {"current_version", AppConfig.CURRENT_VERSION},
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
                {"current_version", AppConfig.CURRENT_VERSION},
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
                {"current_version", AppConfig.CURRENT_VERSION},
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