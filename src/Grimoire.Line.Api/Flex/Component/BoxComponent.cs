using System.Collections.Generic;

namespace Grimoire.Line.Api.Flex.Component
{
    public record BoxComponent : BaseComponent
    {
        public BoxComponent()
        {
            ComponentType = ComponentType.Box;
        }

        public Layout Layout { get; set; }
        public List<BaseComponent> Contents { get; set; }
        public string BackgroundColor { get; set; } = "#00000000";
        public string BorderColor { get; set; }
        public string BorderWidth { get; set; }
        public string CornerRadius { get; set; } = "none";
        public string Width { get; set; }
        public string Height { get; set; }
        public int Flex { get; set; }
        public string Spacing { get; set; } = "none";
        public string Margin { get; set; }
        public string PaddingAll { get; set; }
        public string PaddingTop { get; set; }
        public string PaddingBottom { get; set; }
        public string PaddingStart { get; set; }
        public string PaddingEnd { get; set; }
        public Position Position { get; set; }
        public string OffsetTop { get; set; }
        public string OffsetBottom { get; set; }
        public string OffsetStart { get; set; }
        public string OffsetEnd { get; set; }
        public Action.BaseAction Action { get; set; }
        public string JustifyContent { get; set; }
        public string AlignItems { get; set; }
        public Background Background { get; set; }
    }
}