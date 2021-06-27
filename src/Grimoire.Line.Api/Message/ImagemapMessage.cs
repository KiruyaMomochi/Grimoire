using System.Collections.Generic;

namespace Grimoire.Line.Api.Message
{
    public record ImagemapMessage : BaseMessage
    {
        public ImagemapMessage() { MessageType = MessageType.Imagemap; }
        public string BaseUrl { get; set; }
        public string AltText { get; set; }
        public ImageMap.BaseSize BaseSize { get; set; }

        public ImageMap.Video Video { get; set; }
        public List<ImageMap.BaseAction> Actions { get; set; }
    }
}