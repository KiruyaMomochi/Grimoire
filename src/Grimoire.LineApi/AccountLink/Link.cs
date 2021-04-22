namespace Grimoire.LineApi.AccountLink
{
    public record Link
    {
        public string Result { get; set; }
        public string Nonce { get; set; }
    }
}