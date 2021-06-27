namespace Grimoire.Line.Api.Message
{
    public record FlexMessage : BaseMessage
    {
        public FlexMessage()
        {
            MessageType = MessageType.Flex;
        }

        public string AltText { get; set; }
        public string Contents { get; set; } // Too many things, we will use json text directly for now
    }
}