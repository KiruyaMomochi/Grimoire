using System.Collections.Generic;

namespace Grimoire.Line.Api.Message.Template.Carousel
{
    public record Column
    {
        public string ThumbnailImageUrl { get; set; }
        public string ImageBackgroundColor { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Action.BaseAction DefaultAction { get; set; }
        public List<Action.BaseAction> Actions { get; set; }
    }
}