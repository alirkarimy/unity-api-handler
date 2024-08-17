using Alec.Core;
using System;
using System.Collections.Generic;

namespace Alec.Api
{
    public class RegisterAPI : IPostRequest<Response>
    {
       
        public string Route => "/users/register";

        public string ApiVersion => "2";

        public string JSONData { get; set; } = default;
        public Dictionary<string, string> FormData { get; set; } = default;

        public void OnResponse(Response result)
        {
            if (result.Status == ResponseStatus.Successful)
            {
                AlecListener.OnUserRegisterRecived?.Invoke(result.Message);
            }
            else
            {
                AlecListener.OnUserRegisterFailed?.Invoke(result.Status, result.Message);
            }
        }

       
    }


}
