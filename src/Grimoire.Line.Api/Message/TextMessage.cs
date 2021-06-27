using System.Collections.Generic;

namespace Grimoire.Line.Api.Message
{
    public record TextMessage : BaseMessage
    {
        public TextMessage() { MessageType = MessageType.Text; }
        public string Text { get; set; }
        public List<Emoji> Emojis { get; set; }
    }
}
