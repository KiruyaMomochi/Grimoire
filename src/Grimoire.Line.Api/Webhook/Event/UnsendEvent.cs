namespace Grimoire.Line.Api.Webhook.Event
{
    public record UnsendEvent : BaseEvent
    {
        public Unsend Unsend { get; set; }
    }
}