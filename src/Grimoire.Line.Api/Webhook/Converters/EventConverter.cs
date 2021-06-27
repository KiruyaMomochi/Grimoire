using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Grimoire.Line.Api.Webhook.Event;

namespace Grimoire.Line.Api.Webhook.Converters
{
    /// <summary>
    /// Convert json string to Event object
    /// </summary>
    /// <remarks>
    /// The first property must be "type".
    /// </remarks>
    public class EventConverter : JsonConverter<BaseEvent>
    {
        public override bool CanConvert(System.Type typeToConvert)
            => typeof(BaseEvent).IsAssignableFrom(typeToConvert);

        public override BaseEvent Read(ref Utf8JsonReader reader, System.Type typeToConvert,
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
                if (!Enum.TryParse<EventType>(typeString, out var type))
                    throw new JsonException($"Unknown type {type}");

                options = Options.SerializerOption;
                return type switch
                {
                    EventType.Message => JsonSerializer.Deserialize<MessageEvent>(ref reader, options),
                    EventType.Unsend => JsonSerializer.Deserialize<UnsendEvent>(ref reader, options),
                    EventType.Follow => JsonSerializer.Deserialize<FollowEvent>(ref reader, options),
                    EventType.Unfollow => JsonSerializer.Deserialize<UnFollowEvent>(ref reader, options),
                    EventType.Join => JsonSerializer.Deserialize<JoinEvent>(ref reader, options),
                    EventType.Leave => JsonSerializer.Deserialize<LeaveEvent>(ref reader, options),
                    EventType.MemberJoined => JsonSerializer.Deserialize<MemberJoinEvent>(ref reader, options),
                    EventType.MemberLeft => JsonSerializer.Deserialize<MemberLeaveEvent>(ref reader, options),
                    EventType.Postback => JsonSerializer.Deserialize<PostbackEvent>(ref reader, options),
                    EventType.VideoPlayComplete => JsonSerializer.Deserialize<VideoViewingCompleteEvent>(ref reader, options),
                    EventType.Beacon => JsonSerializer.Deserialize<BeaconEvent>(ref reader, options),
                    EventType.AccountLink => JsonSerializer.Deserialize<AccountLinkEvent>(ref reader, options),
                    EventType.Things => JsonSerializer.Deserialize<ThingsEvent>(ref reader, options),
                    _ => throw new JsonException()
                };
            }
            
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
        {
            options = Options.SerializerOption;
            JsonSerializer.Serialize<object>(writer, value, options);
        }
    }
}