namespace Grimoire.Line.Api.Webhook.Source
{
    public record RoomSource : BaseSource
    {
        public string RoomId { get; set; }
    }
}