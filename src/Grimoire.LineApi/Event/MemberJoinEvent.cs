using Grimoire.LineApi.Member;

namespace Grimoire.LineApi.Event
{
    public record MemberJoinEvent : BaseEvent
    {
        public MemberList Joined { get; set; }
        public string ReplyToken { get; set; }
    }
}