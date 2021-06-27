using System.Text.Json.Serialization;
using Grimoire.Line.Api.Webhook.Converters;

namespace Grimoire.Line.Api.Webhook.Message
{
    [JsonConverter(typeof(MessageConverter))]
    public abstract record BaseMessage
    {
        public string Id { get; set; }
        [JsonPropertyName("type")] public MessageType MessageType { get; set; }
    }
}
