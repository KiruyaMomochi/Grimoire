using Grimoire.Line.Api.Webhook.Things;

namespace Grimoire.Line.Api.Webhook.Event
{
    public record ThingsEvent : BaseEvent
    {
        public string ReplyToken { get; set; }
        public BaseThings Things { get; set; }
    }
}