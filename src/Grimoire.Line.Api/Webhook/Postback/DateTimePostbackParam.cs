using System;
using System.Text.Json.Serialization;
using Grimoire.Line.Api.Converters;

namespace Grimoire.Line.Api.Webhook.Postback
{
    public record DateTimePostbackParam : BasePostbackParam
    {
        [JsonConverter(typeof(DateTimeParseUpperTConverter))]
        [JsonPropertyName("datetime")]
        public DateTime DateTime { get; set; }
    }
}