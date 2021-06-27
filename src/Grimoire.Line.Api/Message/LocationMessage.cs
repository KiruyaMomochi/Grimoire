namespace Grimoire.Line.Api.Message
{
    public record LocationMessage : BaseMessage
    {
        public LocationMessage() { MessageType = MessageType.Location; }
        public string Title { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}