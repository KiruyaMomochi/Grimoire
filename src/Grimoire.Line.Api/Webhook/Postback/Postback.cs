namespace Grimoire.Line.Api.Webhook.Postback
{
    public record Postback
    {
        public string Data { get; set; }
        public BasePostbackParam Params { get; set; }
    }
}