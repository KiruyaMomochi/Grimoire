namespace Grimoire.Line.Api.Webhook.Beacon
{
    public record Beacon
    {
        public string Hwid { get; set; }
        public string Dm { get; set; }
        public BeaconType Type { get; set; }
    }
}