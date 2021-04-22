namespace Grimoire.LineApi.Event
{
    public record Unsend : BaseEvent
    {
        public string MessageId { get; set; }
    }
}