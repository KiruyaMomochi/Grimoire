using System;
using System.Text.Json.Serialization;
using Grimoire.Line.Api.Converters;

namespace Grimoire.Line.Api.Action
{
    public record DatetimePickerAction : BaseAction
    {
        public DatetimePickerAction()
        {
            ActionType = ActionType.Datetimepicker;
        }
        public string Label { get; set; }
        public string Data { get; set; }
        public Datetime.Mode Mode { get; set; }

        [JsonConverter(typeof(DateTimeParseUpperTConverter))]
        public DateTime Initial { get; set; }

        [JsonConverter(typeof(DateTimeParseUpperTConverter))]
        public DateTime Max { get; set; }

        [JsonConverter(typeof(DateTimeParseUpperTConverter))]
        public DateTime Min { get; set; }
    }
}