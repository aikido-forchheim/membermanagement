using System;
using System.Collections.Generic;
using System.Text;

namespace AVF.StandardLibrary.Extensions
{
    public static class ObjectExtensions
    {
        public static string Serialize<T>(this T t)
        {
            string serializedString = Newtonsoft.Json.JsonConvert.SerializeObject(t);
            return serializedString;
        }

        public static T Deserialize<T>(this string value)
        {
            T t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
            return t;
        }
    }
}
