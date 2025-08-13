using Alec.Core;
using System;
using System.Collections.Generic;

namespace Alec.Api
{
    public class GetUserProfileApi : IGetRequest<Response<UserDataModel>>
    {
        public string Route => "/auth";

        public string ApiVersion => "1";

        public Dictionary<string, string> Params { get; set; } = default;

        public void OnResponse(Response<UserDataModel> result)
        {
            if (result.Status == ResponseStatus.SUCCEED)
            {
                AlecListener.OnUserProfileRecived?.Invoke(result.Body);
            }
            else
            {
                AlecListener.OnUserProfileFailed?.Invoke(result.Status, result.Message);
            }
        }
    }

    [Serializable]
    public class UserDataModel
    {
        public User user;
        public Session session;
    }
    [Serializable]

    public class User
    {
        public string id;
        public string name;
        public string mobile;
        public bool isActive;
        public DateTime createdAt;
        public DateTime updatedAt;
    }
    [Serializable]

    public class Session
    {
        public string id;
        public string refId;
        public DateTime refreshExpiresAt;
        public string firebaseToken;
        public string appVersion;
        public string os;
        public string platform;
        public string ip;
        public DateTime createdAt;
        public DateTime updatedAt;
    }
}

