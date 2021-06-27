using Grimoire.Line.Api.Webhook.Message;

namespace Grimoire.Line.Api.Webhook.Event
{
    public record MessageEvent : BaseEvent
    {
        public string ReplyToken { get; set; }
        public BaseMessage Message { get; set; }
    }
}