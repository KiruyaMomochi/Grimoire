using System;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Grimoire.Line.Api.Message.Template
{
    public abstract record BaseTemplate
    {
        [JsonPropertyName("type")] public TemplateType TemplateType { get; protected set; }
    }
}

namespace Grimoire.Line.Api.Message.Template.ImageCarousel
{
    public record Column
    {
        public string ImageUrl { get; set; }
        public Action.BaseAction Action { get; set; }
    }
}