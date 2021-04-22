using Grimoire.LineApi.Message;
using Grimoire.LineApi.Source;

namespace Grimoire.LineApi.Event
{
    public record MessageEvent : BaseEvent
    {
        public string ReplyToken { get; set; }
        public BaseMessage Message { get; set; }
    }
}