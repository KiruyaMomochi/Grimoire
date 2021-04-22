using System.Text.Json.Serialization;
using Grimoire.LineApi.Converters;

namespace Grimoire.LineApi.ContentProvider
{
    [JsonConverter(typeof(ContentConverter))]
    public abstract record BaseContentProvider
    {
        [JsonPropertyName("type")]
        public ContentProviderType ContentType { get; set; }
    }
}