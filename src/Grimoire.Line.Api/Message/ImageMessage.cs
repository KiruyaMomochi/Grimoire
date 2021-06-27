namespace Grimoire.Line.Api.Message
{
    public record ImageMessage : BaseMessage
    {
        public ImageMessage() { MessageType = MessageType.Image; }
        public string OriginalContentUrl { get; set; }
        public string PreviewImageUrl { get; set; }
    }
}