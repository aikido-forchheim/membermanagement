using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AVF.MemberManagement.StandardLibrary.Converters
{
    public class DateTimeStringToNullableDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            DateTime dt = DateTime.MinValue;

            if (reader.TokenType == JsonToken.String)
            {
                if (DateTime.TryParse(reader.Value.ToString(), out dt))
                {
                    return dt;
                }
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JValue jValue;

            if (value == null)
            {
                jValue = new JValue("0000-00-00T00:00:00");
            }
            else
            {
                var s = ((DateTime)value).ToString("s");
                jValue = new JValue(s);
            }

            jValue.WriteTo(writer);
        }
    }
}
