using Alec.Core;
using System;
using System.Collections.Generic;

namespace Alec.Api
{
    public class AppConfigApi : IPostRequest<Response<AppConfigModel>>
    {

        public string Route => "/apps/config";

        public string ApiVersion => "2";

        public string JSONData { set; get; } = default;
        public Dictionary<string, string> FormData { set; get; } = default;

        public void OnResponse(Response<AppConfigModel> result)
        {
            if (result.Status == ResponseStatus.Successful)
            {
                AlecListener.OnAppConfigReceived?.Invoke(result.Body);
            }
            else
            {
                AlecListener.OnAppConfigFailed?.Invoke(result.Status, result.Message);
            }
        }

    }
    
    [Serializable]
    public class AppConfigModel 
    {
        public KeyValuePair[] Popups;
        public double now = 1622607268;
        public bool app_force_update = false;
        public string app_last_version = "1.0.9";
        public string app_download_link = "https://play.google.com/store/apps/details?id=com.company.game";
        public string app_id = "1";
        public string bundles_url_android = "https://android-bundles-v2-0.domain.com";
        public string bundles_url_ios = "https://ios-bundles-v2-0.domain.com";
    }


}
