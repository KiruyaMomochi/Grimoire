namespace Grimoire.Line.Api.Webhook.Event
{
    public record FollowEvent : BaseEvent
    {
        public string ReplyToken { get; set; }
    }
}