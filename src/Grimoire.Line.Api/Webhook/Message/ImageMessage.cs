using Grimoire.Line.Api.Webhook.ContentProvider;

namespace Grimoire.Line.Api.Webhook.Message
{
    public record ImageMessage : BaseMessage
    {
        public BaseContentProvider BaseContentProvider { get; set; }
    }
}