using System.Text.Json;
using System.Text.Json.Serialization;

namespace Grimoire.Line.Api.Tests.Webhook
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