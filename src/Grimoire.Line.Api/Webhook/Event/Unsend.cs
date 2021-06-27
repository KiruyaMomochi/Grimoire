namespace Grimoire.Line.Api.Webhook.Event
{
    public record Unsend : BaseEvent
    {
        public string MessageId { get; set; }
    }
}