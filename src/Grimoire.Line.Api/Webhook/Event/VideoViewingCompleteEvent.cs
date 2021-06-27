using Grimoire.Line.Api.Webhook.Video;

namespace Grimoire.Line.Api.Webhook.Event
{
    public record VideoViewingCompleteEvent : BaseEvent
    {
        public string ReplyToken { get; set; }
        public VideoPlayComplete VideoPlayComplete { get; set; }
    }
}