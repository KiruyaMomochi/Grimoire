namespace Grimoire.LineApi.ContentProvider
{
    public record ExternalContentProvider : BaseContentProvider
    {
        public string OriginalContentUrl { get; set; }
        public string PreviewImageUrl { get; set; }
    }
}