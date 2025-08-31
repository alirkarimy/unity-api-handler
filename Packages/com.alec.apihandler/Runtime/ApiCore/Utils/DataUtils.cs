using Best.HTTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Alec.Core
{
    public static class DataUtils
    {
        public static string ConvertToJSON(Dictionary<string, string> dictionary)
        {
            return JsonConvert.SerializeObject(dictionary);
        }

        public static string ConvertToJSON(Dictionary<string,List<string>> dictionary)
        {
            return JsonConvert.SerializeObject(dictionary);
        }

        public static string ConvertToJSON<T>(T model)
        {
            //TODO : 
            return JsonUtility.ToJson(model);
        }
        public static string GetCurlCommand(HTTPRequest request,Dictionary<string,string > headers)
        {
            StringBuilder curlCommand = new StringBuilder("curl");

            // Add URL
            curlCommand.Append($" -X {request.MethodType} '{request.Uri.ToString()}'");

            //// Add Headers
            //foreach (string header in headers.Keys)
            //{
            //    curlCommand.Append($" -H '{header}: {request.GetFirstHeaderValue(header)}'");
            //}

            //// Add POST/PUT data (if any)
            //if (request.UploadSettings!= null && request.UploadSettings.UploadStream!= null && request.UploadSettings.UploadStream.Length > 0)
            //{
            //    string postData = Encoding.UTF8.GetString(request.UploadSettings.UploadStream);
            //    curlCommand.Append($" --data-raw '{postData}'");
            //}

            return curlCommand.ToString();
        }
        public static void GetValue(KeyValuePair[] pairs, string key, ref string value)
        {
            string val = GetValue(pairs, key);
            value = string.IsNullOrEmpty(val) ? value : val;
        }
     
        public static void GetValue(KeyValuePair[] pairs, string key, ref int value)
        {
            try { value = int.Parse(GetValue(pairs, key)); } catch (Exception ex) { };
        }

        public static void GetValue(KeyValuePair[] pairs, string key, ref bool value)
        {
            try { value = int.Parse(GetValue(pairs, key)) == 1; } catch (Exception ex) { };
        }
      
        private static string GetValue(KeyValuePair[] pairs, string key)
        {
            if (pairs == null) return string.Empty;

            for (int i = 0; i < pairs.Length; i++)
            {
                if (pairs[i].key.Equals(key))
                    return pairs[i].value;
            }
            return string.Empty;
        }
        public static bool IsNullOrEmpty(params string[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (string.IsNullOrEmpty(parameters[i]))
                    return true;
            }
            return false;
        }
    }

    [Serializable]
    public class KeyValuePair
    {
        public string key;
        public string value;
    }
}
