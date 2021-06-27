using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Grimoire.Line.Api.Converters
{
    public class DateTimeParseLowerTConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString() ?? "");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString("yyyy-MM-ddtHH:mm", CultureInfo.InvariantCulture));
    }
}