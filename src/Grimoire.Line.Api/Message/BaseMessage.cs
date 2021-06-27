using System.Text.Json.Serialization;
using Grimoire.Line.Api.Message.Converters;

namespace Grimoire.Line.Api.Message
{
    [JsonConverter(typeof(MessageConverter))]
    public abstract record BaseMessage
    {
        [JsonPropertyName("type")] public MessageType MessageType { get; protected set; }
        public Sender Sender { get; set; }
        public Reply.QuickReply QuickReply { get; set; }
    }
}
