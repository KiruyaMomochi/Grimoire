using Grimoire.Line.Api.Webhook.ContentProvider;

namespace Grimoire.Line.Api.Webhook.Message
{
    public record AudioMessage : BaseMessage
    {
        public BaseContentProvider ContentProvider { get; set; }
        public int Duration { get; set; }
    }
}