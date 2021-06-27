using System.Collections.Generic;

namespace Grimoire.Line.Api.Webhook.Message
{
    public record StickerMessage : BaseMessage
    {
        public string PackageId { get; set; }
        public string StickerId { get; set; }
        public string StickerResourceType { get; set; }
        public List<string> Keywords { get; set; }
    }
}