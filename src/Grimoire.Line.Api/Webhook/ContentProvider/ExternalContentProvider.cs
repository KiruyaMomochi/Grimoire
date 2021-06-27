namespace Grimoire.Line.Api.Webhook.ContentProvider
{
    public record ExternalContentProvider : BaseContentProvider
    {
        public string OriginalContentUrl { get; set; }
        public string PreviewImageUrl { get; set; }
    }
}