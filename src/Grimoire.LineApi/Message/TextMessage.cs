using System.Collections.Generic;

namespace Grimoire.LineApi.Message
{
    public record TextMessage : BaseMessage
    {
        public string Text { get; set; }
        public List<Emoji> Emojis { get; set; }
        public Mention Mention { get; set; }
    }
}