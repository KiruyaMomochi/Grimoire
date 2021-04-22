using System;
using System.Text.Json.Serialization;
using Grimoire.LineApi.Converters;

namespace Grimoire.LineApi.Postback
{
    public record DateTimePostbackParam : BasePostbackParam
    {
        [JsonConverter(typeof(DateTimeParseConverter))]
        [JsonPropertyName("datetime")]
        public DateTime DateTime { get; set; }
    }
}