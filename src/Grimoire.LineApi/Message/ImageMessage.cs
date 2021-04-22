using Grimoire.LineApi.ContentProvider;

namespace Grimoire.LineApi.Message
{
    public record ImageMessage : BaseMessage
    {
        public BaseContentProvider BaseContentProvider { get; set; }
    }
}