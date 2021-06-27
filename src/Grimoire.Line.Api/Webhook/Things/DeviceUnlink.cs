namespace Grimoire.Line.Api.Webhook.Things
{
    public record DeviceUnlink : BaseThings
    {
        public string DeviceId { get; set; }
    }
}