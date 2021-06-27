using System.Collections.Generic;

namespace Grimoire.Line.Api.Message.Template
{
    public record CarouselTemplate : BaseTemplate
    {
        public CarouselTemplate()
        {
            TemplateType = TemplateType.Carousel;
        }

        public List<Carousel.Column> Columns { get; set; }
        public ImageAspectRatio ImageAspectRatio { get; set; }
        public ImageSize ImageSize { get; set; }
    }
}