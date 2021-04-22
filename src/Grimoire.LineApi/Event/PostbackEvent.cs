namespace Grimoire.LineApi.Event
{
    public record PostbackEvent : BaseEvent
    {
        public Postback.Postback Postback { get; set; }
    }
}