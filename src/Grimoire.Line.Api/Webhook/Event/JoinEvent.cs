namespace Grimoire.Line.Api.Webhook.Event
{
    public record JoinEvent : BaseEvent
    {
        public string ReplyToken { get; set; }
    }
}