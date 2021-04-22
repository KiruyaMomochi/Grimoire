using System.Text.Json;
using System.Text.Json.Serialization;

namespace Grimoire.LineApi.Tests
{
    public static class Json
    {
        public static readonly JsonSerializerOptions SerializerOption =
            new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
    }
}