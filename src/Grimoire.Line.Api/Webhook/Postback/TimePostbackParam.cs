namespace Grimoire.Line.Api.Webhook.Postback
{
    public record TimePostbackParam : BasePostbackParam
    {
        public string Time { get; set; }
    }
}