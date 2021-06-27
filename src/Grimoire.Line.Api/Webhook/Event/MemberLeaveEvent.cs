using Grimoire.Line.Api.Webhook.Source;

namespace Grimoire.Line.Api.Webhook.Event
{
    public record MemberLeaveEvent : BaseEvent
    {
        public MemberList Left { get; set; }
    }
}