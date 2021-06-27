using System.Text.Json.Serialization;
using Grimoire.Line.Api.Webhook.Converters;

namespace Grimoire.Line.Api.Webhook.Postback
{
    [JsonConverter(typeof(PostbackParamConverter))]
    public abstract record BasePostbackParam
    {
    }
}