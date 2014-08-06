using System;
using System.Configuration;

namespace HtmlToPdf.Parser
{
    public static class AppSettingsHelper
    {
        public static string GetValue(string key)
        {
            return GetValue<string>(key);
        }

        public static T GetValue<T>(string key, T defaultValue = default(T))
        {
            var value = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrEmpty(value))
                return defaultValue;

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}