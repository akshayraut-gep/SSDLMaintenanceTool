using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SSDLMaintenanceTool.Helpers
{
    public static class StringExtensions
    {
        public static bool HasContent(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
        public static int GetIntOrDefault(this string value)
        {
            int parsedValue;
            int.TryParse(value, out parsedValue);
            return parsedValue;
        }

        public static long GetLongOrDefault(this string value)
        {
            long parsedValue;
            long.TryParse(value, out parsedValue);
            return parsedValue;
        }

        public static string MapString(this string value, IDictionary<string, object> parameters)
        {
            if (!value.HasContent())
                return value;

            if (parameters == null || parameters.Count == 0)
                return value;

            StringBuilder finalValue = new StringBuilder(value);
            foreach (var item in parameters)
            {
                if (value.Contains(item.Key))
                    finalValue.Replace("{" + item.Key + "}", item.Value?.ToString() ?? "");
            }
            return finalValue.ToString();
        }

        public static bool IsJsonValid(this string txt)
        {
            try
            {
                return ((JsonSerializer.Deserialize(txt, typeof(object)) != null) && (JsonSerializer.Deserialize(txt, typeof(object)).GetType().Name != null)
                  && ((JsonSerializer.Deserialize(txt, typeof(object)).GetType().Name == "JObject")
                  || (JsonSerializer.Deserialize(txt, typeof(object)).GetType().Name == "JArray")));
            }
            catch { }

            return false;
        }

        public static bool GetBoolOrDefault(this string value)
        {
            var result = false;
            try
            {
                Boolean.TryParse(value, out result);
            }
            catch
            {
                return result;
            }
            return result;
        }
    }
}
