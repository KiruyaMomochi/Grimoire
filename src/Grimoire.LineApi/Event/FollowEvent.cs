namespace Grimoire.LineApi.Event
{
    public record FollowEvent : BaseEvent
    {
        public string ReplyToken { get; set; }
    }
}