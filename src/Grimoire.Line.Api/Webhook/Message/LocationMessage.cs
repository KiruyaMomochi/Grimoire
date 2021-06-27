namespace Grimoire.Line.Api.Webhook.Message
{
    public record LocationMessage : BaseMessage
    {
        public string Title { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}