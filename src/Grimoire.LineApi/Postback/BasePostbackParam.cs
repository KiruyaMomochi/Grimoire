using System.Text.Json.Serialization;
using Grimoire.LineApi.Converters;

namespace Grimoire.LineApi.Postback
{
    [JsonConverter(typeof(PostbackParamConverter))]
    public abstract record BasePostbackParam
    {
    }
}