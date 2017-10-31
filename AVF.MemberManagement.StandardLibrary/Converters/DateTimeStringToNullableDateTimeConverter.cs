using System;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Converters
{
    public class IntStringToBooleanConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int i = 0;

            if (reader.TokenType == JsonToken.String)
            {
                if (int.TryParse(reader.Value.ToString(), out i))
                {
                    if (i > 0) return true;
                }
            }

            return false;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
