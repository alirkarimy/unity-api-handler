using Alec.Core;
using System;
using System.Collections.Generic;

namespace Alec.Api
{
    public class GetUserProfileApi : IGetRequest<Response<GetUserProfileApiModel>>
    {
        public string Route => "/users";

        public string ApiVersion => "1";

        public Dictionary<string, string> Params { get; set; } = default;

        public void OnResponse(Response<GetUserProfileApiModel> result)
        {
            if (result.Status == ResponseStatus.Successful)
            {
                AlecListener.OnGetUserProfileRecived?.Invoke(result.Body);
            }
            else
            {
                AlecListener.OnGetUserProfileFailed?.Invoke(result.Status, result.Message);
            }
        }
    }

    [Serializable]
    public class GetUserProfileApiModel 
    {
        public int id;
        public string mobile_number_prefix;
        public string mobile_number;
        public string first_name;
        public string last_name;
        public int city_id;
        public string city;
        public string country;
        public int birthday;
        public int gender;
    }

}
