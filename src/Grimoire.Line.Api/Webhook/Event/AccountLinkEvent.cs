using Grimoire.Line.Api.Webhook.AccountLink;

namespace Grimoire.Line.Api.Webhook.Event
{
    public record AccountLinkEvent : BaseEvent
    {
        public string ReplyToken { get; set; }
        public Link Link { get; set; }
    }
}