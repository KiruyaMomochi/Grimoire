namespace Grimoire.Line.Api.Message
{
    public record AudioMessage : BaseMessage
    {
        public AudioMessage() { MessageType = MessageType.Audio; }
        public string OriginalContentUrl { get; set; }
        public int Duration { get; set; }
    }
}