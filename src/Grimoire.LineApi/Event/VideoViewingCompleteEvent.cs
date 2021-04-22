using Grimoire.LineApi.Video;

namespace Grimoire.LineApi.Event
{
    public record VideoViewingCompleteEvent : BaseEvent
    {
        public string ReplyToken { get; set; }
        public VideoPlayComplete VideoPlayComplete { get; set; }
    }
}