namespace Grimoire.Line.Api.Message
{
    public record StickerMessage : BaseMessage
    {
        public StickerMessage() { MessageType = MessageType.Sticker; }
        public string PackageId { get; set; }
        public string StickerId { get; set; }
    }
}