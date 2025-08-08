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
        public User User { get; set; }
        public Session Session { get; set; }
    }
    [Serializable]

    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    [Serializable]

    public class Session
    {
        public string Id { get; set; }
        public string RefId { get; set; }
        public DateTime RefreshExpiresAt { get; set; }
        public string FirebaseToken { get; set; }
        public string AppVersion { get; set; }
        public string Os { get; set; }
        public string Platform { get; set; }
        public string Ip { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

