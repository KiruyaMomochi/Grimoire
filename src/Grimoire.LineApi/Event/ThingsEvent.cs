using Grimoire.LineApi.Things;

namespace Grimoire.LineApi.Event
{
    public record ThingsEvent : BaseEvent
    {
        public string ReplyToken { get; set; }
        public BaseThings Things { get; set; }
    }
}