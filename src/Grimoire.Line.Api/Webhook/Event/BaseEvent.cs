using System;
using System.Text.Json.Serialization;
using Grimoire.Line.Api.Webhook.Converters;
using Grimoire.Line.Api.Webhook.Source;

namespace Grimoire.Line.Api.Webhook.Event
{
    [JsonConverter(typeof(EventConverter))]
    public abstract record BaseEvent
    {
        [JsonPropertyName("type")] public string EventType { get; set; }
        public Mode Mode { get; set; }
        
        [JsonConverter(typeof(UnixMillisecondsConverter))]
        public DateTimeOffset Timestamp { get; set; }
        public BaseSource Source { get; set; }
    }
}