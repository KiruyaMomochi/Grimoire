using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Grimoire.Line.Api.Webhook.Message;

namespace Grimoire.Line.Api.Message.Converters
{
    /// <summary>
    /// Convert json string to Message object
    /// </summary>
    /// <remarks>
    /// The first property must be "type".
    /// </remarks>
    public class MessageConverter : JsonConverter<BaseMessage>
    {
        public override bool CanConvert(System.Type typeToConvert)
            => typeof(BaseMessage).IsAssignableFrom(typeToConvert);

        public override BaseMessage Read(ref Utf8JsonReader reader, System.Type typeToConvert,
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
                if (!Enum.TryParse<MessageType>(typeString, out var type))
                    throw new JsonException($"Unknown type {type}");
                
                options = Options.SerializerOption;
                return type switch
                {
                    MessageType.Text => JsonSerializer.Deserialize<TextMessage>(ref reader, options),
                    MessageType.Image => JsonSerializer.Deserialize<ImageMessage>(ref reader, options),
                    MessageType.Video => JsonSerializer.Deserialize<VideoMessage>(ref reader, options),
                    MessageType.Audio => JsonSerializer.Deserialize<AudioMessage>(ref reader, options),
                    MessageType.Location => JsonSerializer.Deserialize<LocationMessage>(ref reader, options),
                    MessageType.Sticker => JsonSerializer.Deserialize<StickerMessage>(ref reader, options),
                    MessageType.Flex => JsonSerializer.Deserialize<FlexMessage>(ref reader, options),
                    MessageType.Imagemap => JsonSerializer.Deserialize<ImagemapMessage>(ref reader, options),
                    MessageType.Template => JsonSerializer.Deserialize<TemplateMessage>(ref reader, options),
                    _ => throw new JsonException()
                };
            }
            
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, BaseMessage value, JsonSerializerOptions options)
        {
            options = Options.SerializerOption;
            JsonSerializer.Serialize<object>(writer, value, options);
        }
    }
}