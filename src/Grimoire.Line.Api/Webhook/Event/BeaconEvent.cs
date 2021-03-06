namespace Grimoire.Line.Api.Webhook.Event
{
    public record BeaconEvent : BaseEvent
    {
        public string ReplyToken { get; set; }
        public Beacon.Beacon Beacon { get; set; }
    }
}