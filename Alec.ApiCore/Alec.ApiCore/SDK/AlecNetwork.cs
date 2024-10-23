﻿using Alec.Api;
using Alec.Core;
using System;
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

            if (IsNullOrEmpty(mono, baseUrl, clinetId, clientSecret, deviceUUID, storeName, apkVersion, osVersion, osName))
            {
                AlecListener.WebCoreInitializeFailed?.Invoke("Not Valid Arguments To Intialize WebCore");
            }
            AppConfig.OS_NAME = osName;
            AppConfig.OS_VERSION = osVersion;
            AppConfig.CURRENT_VERSION = apkVersion;
            SetBaseUrl(baseUrl);
           
            WebCore.Initialize(mono,new Dictionary<string, string> {

                { "store-name", storeName },
                { "Accept", "application/json" },
                { "client-id", clinetId },
                { "client-secret", clientSecret },
                { "device-uuid", deviceUUID }

            });

            AlecListener.WebCoreInitialized?.Invoke();

        }
        public static void SetBaseUrl(string baseUrl)
        {
            UrlUtils.SetBaseUrl(baseUrl);
        }
        public static void GetAppConfig(string connectionData)
        {
            AppConfigApi api = new AppConfigApi();

            api.FormData = new Dictionary<string, string>
            {
                { "current_version", AppConfig.CURRENT_VERSION },
                { "os_version", AppConfig.OS_VERSION },
                { "os_name", AppConfig.OS_NAME }
            };

            if (!string.IsNullOrEmpty(connectionData))
                api.FormData.Add("connectoindata", connectionData);
            
            WebCore.Post<Response<AppConfigModel>>(api);
        }
        public static void RegisterUser(string moblieNumber, string mobilePrefix)
        {
            RegisterAPI api = new RegisterAPI();
            api.FormData = new Dictionary<string, string>
            {
                { "mobile_number", moblieNumber },
                { "mobile_number_prefix", mobilePrefix }
            };
            WebCore.Post(api);
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
        public static void SetToken(string token, string tokenType)
        {
            AddHeader("Authorization" , string.Format("{0} {1}", tokenType, token));
        }
        public static void AddHeader(string headerKey, string headerValue)
        {
            WebCore.AddHeaders(new Dictionary<string, string>
            {
                {headerKey,headerValue}
            });
        }
        private static bool IsNullOrEmpty(MonoBehaviour mono, params string[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (string.IsNullOrEmpty(parameters[i]))
                    return true;
            }
            return !mono;
        }

    }
}