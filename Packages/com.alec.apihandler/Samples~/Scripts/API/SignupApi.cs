using Alec.Core;
using System;
using System.Collections.Generic;

namespace Alec.Api
{
    public class SignupApi : IPostRequest<Response<UserDataModel>>
    {
        public string Route => "/auth/signup";

        public string ApiVersion => "1";

        public string JSONData { set; get; } = default;
        public Dictionary<string, string> FormData { set; get; } = default;

        public void OnResponse(Response<UserDataModel> result)
        {
            if (result.Status == ResponseStatus.SUCCEED)
            {
                AlecListener.OnSignup?.Invoke(result.Body);
            }
            else
            {
                AlecListener.OnSignupFailed?.Invoke(result.Status, result.Message);
            }
        }

       
    }
    
}