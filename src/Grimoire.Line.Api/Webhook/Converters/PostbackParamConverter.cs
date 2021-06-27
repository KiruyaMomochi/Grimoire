using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Grimoire.Line.Api.Webhook.Postback;

namespace Grimoire.Line.Api.Webhook.Converters
{
    public class PostbackParamConverter : JsonConverter<BasePostbackParam>
    {
        public override bool CanConvert(System.Type typeToConvert)
            => typeof(BasePostbackParam).IsAssignableFrom(typeToConvert);

        public override BasePostbackParam Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();
            var propertyName = reader.GetString();

            reader.Read();
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException();
            var propertyValue = reader.GetString();
            if (propertyValue == null)
                throw new JsonException();

            reader.Read();
            if (reader.TokenType != JsonTokenType.EndObject)
                throw new JsonException();

            return propertyName switch
            {
                "datetime" => new DateTimePostbackParam() {DateTime = DateTime.Parse(propertyValue)},
                "date" => new DatePostbackParam() {Date = propertyValue},
                "time" => new TimePostbackParam() {Time = propertyValue},
                _ => throw new JsonException()
            };
        }

        public override void Write(Utf8JsonWriter writer, BasePostbackParam value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize<object>(writer, value, options);
        }
    }
}