namespace Grimoire.Line.Api.Message.ImageMap
{
    public record Video
    {
        public string OriginalContentUrl { get; set; }
        public string PreviewImageUrl { get; set; }
        public Area Area { get; set; }
        public ExternalLink ExternalLink { get; set; }
    }
}