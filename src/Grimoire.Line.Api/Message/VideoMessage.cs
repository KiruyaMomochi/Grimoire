namespace Grimoire.Line.Api.Message
{
    public record VideoMessage : BaseMessage
    {
        public VideoMessage() { MessageType = MessageType.Video; }
        public string OriginalContentUrl { get; set; }
        public string PreviewImageUrl { get; set; }
        public string TrackingId { get; set; }
    }
}