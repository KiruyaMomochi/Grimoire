namespace Grimoire.Line.Api.Webhook.Event
{
    public record PostbackEvent : BaseEvent
    {
        public Postback.Postback Postback { get; set; }
    }
}