using System.Text.Json.Serialization;

namespace Grimoire.Line.Api.Flex
{
    public abstract record BaseFlex
    {
        [JsonPropertyName("type")] public FlexType FlexType { get; protected set; }
    }
}