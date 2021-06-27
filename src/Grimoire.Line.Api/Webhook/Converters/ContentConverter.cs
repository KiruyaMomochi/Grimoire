using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Grimoire.Line.Api.Webhook.ContentProvider;

namespace Grimoire.Line.Api.Webhook.Converters
{
    /// <summary>
    /// Convert json string to ContentProvider object
    /// </summary>
    /// <remarks>
    /// The first property must be "type".
    /// </remarks>
    public class ContentConverter : JsonConverter<BaseContentProvider>
    {
        public override bool CanConvert(System.Type typeToConvert)
            => typeof(BaseContentProvider).IsAssignableFrom(typeToConvert);

        public override BaseContentProvider Read(ref Utf8JsonReader reader, System.Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            var propertyReader = reader;
            while (propertyReader.Read())
            {
                if (propertyReader.TokenType is JsonTokenType.StartObject or JsonTokenType.StartArray) 
                    propertyReader.Skip();

                if (propertyReader.TokenType != JsonTokenType.PropertyName)
                    continue;

                if (propertyReader.GetString() != "type")
                    continue;
                
                propertyReader.Read();
                if (propertyReader.TokenType != JsonTokenType.String)
                    throw new JsonException();
                
                var typeString = propertyReader.GetString().ToUpperFirst();
                if (!Enum.TryParse<ContentProviderType>(typeString, out var type))
                    throw new JsonException($"Unknown type {type}");

                return type switch
                {
                    ContentProviderType.Line => JsonSerializer.Deserialize<LineContentProvider>(ref reader, options),
                    ContentProviderType.External => JsonSerializer.Deserialize<ExternalContentProvider>(ref reader, options),
                    _ => throw new JsonException()
                };
            }
            
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, BaseContentProvider value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize<object>(writer, value, options);
        }
    }
}