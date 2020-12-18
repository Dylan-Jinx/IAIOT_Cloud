using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace IAIOT_alpha_0_1_1.Converter
{
    /// <summary>
    /// Fuction:Converter Datetime Value
    /// </summary>
    public class DateTimeConverter : DateTimeConverterBase
    {
        private static readonly IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd hh:mm:ss" };
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return dtConverter.ReadJson(reader, objectType, existingValue, serializer);
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            dtConverter.WriteJson(writer, value, serializer);
        }
    }
}
