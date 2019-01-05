using System;
using System.Collections.Generic;

namespace FileSharing.Infrastructure
{
    public class Registry
    {
        private static Dictionary<string, dynamic> keyValuePairs = new Dictionary<string, dynamic>();

        public static void Set(string key, dynamic value)
        {
            keyValuePairs.Add(key, value);
        }

        public static dynamic Get(string key)
        {
            if (!keyValuePairs.ContainsKey(key))
                return null;
            return keyValuePairs[key];
        }

        public static void Delete(string key)
        {
            keyValuePairs.Remove(key);
        }
    }
}