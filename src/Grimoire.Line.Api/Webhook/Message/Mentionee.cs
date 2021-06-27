namespace Grimoire.Line.Api.Webhook.Message
{
    public record Mentionee
    {
        public int Index { get; set; }
        public int Length { get; set; }
        public string UserId { get; set; }
    }
}