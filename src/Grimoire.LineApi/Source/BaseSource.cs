using System.Text.Json.Serialization;
using Grimoire.LineApi.Converters;
using Grimoire.LineApi.Source;

namespace Grimoire.LineApi.Source
{
    [JsonConverter(typeof(SourceConverter))]
    public abstract record BaseSource
    {
        [JsonPropertyName("type")]
        public SourceType SourceType { get; set; }
        public string UserId { get; set; }
    }
}