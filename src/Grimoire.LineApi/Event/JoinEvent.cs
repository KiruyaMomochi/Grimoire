namespace Grimoire.LineApi.Event
{
    public record JoinEvent : BaseEvent
    {
        public string ReplyToken { get; set; }
    }
}