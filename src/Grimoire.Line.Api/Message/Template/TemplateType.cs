namespace Grimoire.Line.Api.Message.Template
{
    public enum TemplateType
    {
        Buttons,
        Confirm,
        Carousel,
#pragma warning disable CA1707 // LINE api uses underline
        Image_carousel
#pragma warning restore CA1707 // Identifiers should not contain underscores
    }
}