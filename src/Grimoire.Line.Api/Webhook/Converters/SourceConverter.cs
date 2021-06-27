using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Grimoire.Line.Api.Webhook.Source;

namespace Grimoire.Line.Api.Webhook.Converters
{
    /// <summary>
    /// Convert json string to Source object
    /// </summary>
    /// <remarks>
    /// The first property must be "type".
    /// </remarks>
    public class SourceConverter : JsonConverter<BaseSource>
    {
        public override bool CanConvert(System.Type typeToConvert)
            => typeof(BaseSource).IsAssignableFrom(typeToConvert);

        public override BaseSource Read(ref Utf8JsonReader reader, System.Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            var propertyReader = reader;
            while (propertyReader.Read())
            {
                if (propertyReader.TokenType == JsonTokenType.StartObject || 
                    propertyReader.TokenType == JsonTokenType.StartArray) 
                    propertyReader.Skip();

                if (propertyReader.TokenType != JsonTokenType.PropertyName)
                    continue;

                if (propertyReader.GetString() != "type")
                    continue;
                
                propertyReader.Read();
                if (propertyReader.TokenType != JsonTokenType.String)
                    throw new JsonException();
                
                var typeString = propertyReader.GetString().ToUpperFirst();
                if (!Enum.TryParse<SourceType>(typeString, out var type))
                    throw new JsonException($"Unknown type {type}");

                return type switch
                {
                    SourceType.User => JsonSerializer.Deserialize<UserSource>(ref reader, options),
                    SourceType.Group => JsonSerializer.Deserialize<GroupSource>(ref reader, options),
                    SourceType.Room => JsonSerializer.Deserialize<RoomSource>(ref reader, options),
                    _ => throw new JsonException()
                };
            }
            
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, BaseSource value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize<object>(writer, value, options);
        }
    }
}