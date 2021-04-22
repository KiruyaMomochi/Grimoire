using Grimoire.LineApi.AccountLink;

namespace Grimoire.LineApi.Event
{
    public record AccountLinkEvent : BaseEvent
    {
        public string ReplyToken { get; set; }
        public Link Link { get; set; }
    }
}