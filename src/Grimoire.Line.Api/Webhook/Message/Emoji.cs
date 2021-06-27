namespace Grimoire.Line.Api.Webhook.Message
{
    public record Emoji
    {
        public int Index { get; set; }
        public int Length { get; set; }
        public string ProductId { get; set; }
        public string EmojiId { get; set; }
    }
}