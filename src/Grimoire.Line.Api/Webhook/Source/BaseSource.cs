using System.Text.Json.Serialization;
using Grimoire.Line.Api.Webhook.Converters;

namespace Grimoire.Line.Api.Webhook.Source
{
    [JsonConverter(typeof(SourceConverter))]
    public abstract record BaseSource
    {
        [JsonPropertyName("type")]
        public SourceType SourceType { get; set; }
        public string UserId { get; set; }
    }
}