using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AVF.MemberManagement.StandardLibrary.Converters
{
    public class LongToBooleanConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
            {
                return false;
            }

            var i = (long)reader.Value;

            return i > 0;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JValue jValue;

            if ((bool)value)
            {
                jValue = new JValue(1);
            }
            else
            {
                jValue = new JValue(0);
            }

            jValue.WriteTo(writer);
        }
    }
}
