namespace Grimoire.Line.Api.Message
{
    public record TemplateMessage : BaseMessage
    {
        public TemplateMessage() { MessageType = MessageType.Template; }

        public string AltText { get; set; }
        public Template.BaseTemplate Template { get; set; }
    }
}