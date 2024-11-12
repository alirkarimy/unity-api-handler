using System;
using System.Collections.Generic;
using UnityEngine;

namespace Alec.Core
{
    public static class DataUtils
    {
        public static string ConvertToJSON(Dictionary<string, string> dictionary)
        {
            //TODO : 
            return "";
        }

        public static string ConvertToJSON<T>(T model)
        {
            //TODO : 
            return JsonUtility.ToJson(model);
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
    }

    [Serializable]
    public class KeyValuePair
    {
        public string key;
        public string value;
    }
}
