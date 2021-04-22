using Grimoire.LineApi.Member;

namespace Grimoire.LineApi.Event
{
    public record MemberLeaveEvent : BaseEvent
    {
        public MemberList Left { get; set; }
    }
}