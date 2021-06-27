using System.Text.Json.Serialization;
using Grimoire.Line.Api.Webhook.Converters;

namespace Grimoire.Line.Api.Webhook.ContentProvider
{
    [JsonConverter(typeof(ContentConverter))]
    public abstract record BaseContentProvider
    {
        [JsonPropertyName("type")]
        public ContentProviderType ContentType { get; set; }
    }
}