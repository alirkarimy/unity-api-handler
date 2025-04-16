using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

namespace Alec.Core
{
    public class UrlUtils
    {
        #region Url Paths

        public static string BASE_URL = "https://dev-api.Alecgames.ir";
        private const string API_VERSION = "/api/v";

        #endregion
        public static string CurrentUrl => BASE_URL;

        public static void SetBaseUrl(string newUrl)
        {
            BASE_URL = newUrl;
        }

        public static string BuildUrl(IRequest req)
        {
            string route = req.Route;
            if (!route.StartsWith("/"))
            {
                route = string.Concat("/", route);
            }
            return string.Concat(BASE_URL, API_VERSION, req.ApiVersion, route);
        }

        public static string EncodeGetUrl(string url, Dictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
                return $"{url}?{string.Join("&", parameters.Select(kvp => $"{kvp.Key}={UnityWebRequest.EscapeURL(kvp.Value)}"))}";
            else
                return url;
        }


    }
}