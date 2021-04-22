namespace Grimoire.LineApi.Message
{
    public record FileMessage : BaseMessage
    {
        public string FileName { get; set; }
        public int FileSize { get; set; }
    }
}