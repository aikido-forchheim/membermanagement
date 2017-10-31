using System;
using Newtonsoft.Json;

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
            throw new NotImplementedException();
        }
    }
}
