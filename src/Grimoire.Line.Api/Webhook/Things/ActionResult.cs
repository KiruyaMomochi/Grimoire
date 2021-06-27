namespace Grimoire.Line.Api.Webhook.Things
{
    public record ActionResult
    {
        public string Type { get; set; }
        public string Data { get; set; }
    }
}