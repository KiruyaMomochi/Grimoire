using System.Collections.Generic;

namespace Grimoire.Line.Api.Message.Template
{
    public record ButtonsTemplate : BaseTemplate
    {
        public ButtonsTemplate() { TemplateType = TemplateType.Buttons; }
        public string ThumbnailImageUrl { get; set; }
        public ImageAspectRatio ImageAspectRatio { get; set; }
        public ImageSize ImageSize { get; set; }
        public string ImageBackgroundColor { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Action.BaseAction DefaultAction { get; set; }
        public List<Action.BaseAction> Actions { get; set; }
    }
}