namespace Grimoire.LineApi.Event
{
    public record UnsendEvent : BaseEvent
    {
        public Unsend Unsend { get; set; }
    }
}