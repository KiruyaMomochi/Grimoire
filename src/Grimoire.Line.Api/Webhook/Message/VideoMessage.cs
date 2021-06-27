using Grimoire.Line.Api.Webhook.ContentProvider;

namespace Grimoire.Line.Api.Webhook.Message
{
    public record VideoMessage : BaseMessage
    {
        public int Duration { get; set; }
        public BaseContentProvider ContentProvider { get; set; }
    }
}