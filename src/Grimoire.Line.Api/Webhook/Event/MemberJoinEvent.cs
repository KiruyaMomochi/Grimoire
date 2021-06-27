using Grimoire.Line.Api.Webhook.Source;

namespace Grimoire.Line.Api.Webhook.Event
{
    public record MemberJoinEvent : BaseEvent
    {
        public MemberList Joined { get; set; }
        public string ReplyToken { get; set; }
    }
}