using System.Text.Json.Serialization;

namespace Grimoire.Line.Api.Message.ImageMap
{
    public abstract record BaseAction
    {
        [JsonPropertyName("type")] public ActionType ActionType { get; protected set; }
    }
}