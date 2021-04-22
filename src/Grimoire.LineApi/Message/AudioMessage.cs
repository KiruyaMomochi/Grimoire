using Grimoire.LineApi.ContentProvider;

namespace Grimoire.LineApi.Message
{
    public record AudioMessage : BaseMessage
    {
        public BaseContentProvider ContentProvider { get; set; }
        public int Duration { get; set; }
    }
}