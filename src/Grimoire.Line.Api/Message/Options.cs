using System.Text.Json;
using System.Text.Json.Serialization;

namespace Grimoire.Line.Api.Message
{
    public static class Options
    {
        public static readonly JsonSerializerOptions SerializerOption =
            new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
    }
}