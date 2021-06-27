namespace Grimoire.Line.Api.Webhook.Postback
{
    public record DatePostbackParam : BasePostbackParam
    {
        public string Date { get; set; }
    }
}