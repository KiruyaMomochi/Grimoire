using Grimoire.LineApi.ContentProvider;

namespace Grimoire.LineApi.Message
{
    public record VideoMessage : BaseMessage
    {
        public int Duration { get; set; }
        public BaseContentProvider ContentProvider { get; set; }
    }
}