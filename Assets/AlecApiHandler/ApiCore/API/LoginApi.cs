using Alec.Core;
using System;
using System.Collections.Generic;

namespace Alec.Api
{
    public class LoginApi : IPostRequest<Response<LoginModel>>
    {
        public string Route => "/oauth/token";

        public string ApiVersion => "2";

        public string JSONData { set; get; } = default;
        public Dictionary<string, string> FormData { set; get; } = default;

        public void OnResponse(Response<LoginModel> result)
        {
            if (result.Status == ResponseStatus.Successful)
            {
                AlecListener.OnLoginRecived?.Invoke(result.Body);
            }
            else
            {
                AlecListener.OnLoginFailed?.Invoke(result.Status, result.Message);
            }
        }

       
    }
    [Serializable]
    public class LoginModel
    {
        public string access_token;
        public string token_type;
        public string refresh_token;


    }
}