using System.Text.Json.Serialization;
using Grimoire.LineApi.Source;

namespace Grimoire.LineApi.Message
{
    [JsonConverter(typeof(MessageConverter))]
    public abstract record BaseMessage
    {
        public string Id { get; set; }
        [JsonPropertyName("type")] public MessageType MessageType { get; set; }
    }
}
