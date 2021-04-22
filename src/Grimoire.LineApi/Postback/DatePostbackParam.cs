using System;
using System.Text.Json.Serialization;
using Grimoire.LineApi.Converters;

namespace Grimoire.LineApi.Postback
{
    public record DatePostbackParam : BasePostbackParam
    {
        public string Date { get; set; }
    }
}