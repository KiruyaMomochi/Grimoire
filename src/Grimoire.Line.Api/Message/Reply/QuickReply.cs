using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Grimoire.Line.Api.Message.Reply
{
    public record QuickReply
    {
        public List<BaseQuickReplyButton> Items { get; set; }
    }

    public abstract record BaseQuickReplyButton
    {
        [JsonPropertyName("type")] public QuickReplyType QuickReplyType { get; set; }
    }

    public record ActionQuickReplyButton : BaseQuickReplyButton
    {
        public ActionQuickReplyButton()
        {
            QuickReplyType = QuickReplyType.Action;
        }

        public string ImageUrl { get; set; }
        public Action.BaseAction Action { get; set; }
    }

    public enum QuickReplyType
    {
        Action
    }
}