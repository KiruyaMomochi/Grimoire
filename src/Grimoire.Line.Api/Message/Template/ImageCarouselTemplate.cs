using System.Collections.Generic;

namespace Grimoire.Line.Api.Message.Template
{
    public record ImageCarouselTemplate: BaseTemplate
    {
        public ImageCarouselTemplate()
        {
            TemplateType = TemplateType.Image_carousel;
        }

        public List<ImageCarousel.Column> Columns { get; set; }
    }
}