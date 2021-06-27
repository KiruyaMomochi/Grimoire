using System.Text.Json.Serialization;

namespace Grimoire.Line.Api.Flex.Component
{
    public abstract record BaseComponent
    {
        [JsonPropertyName("type")] public ComponentType ComponentType { get; protected set; }
    }
}