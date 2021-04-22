namespace Grimoire.LineApi.Postback
{
    public record Postback
    {
        public string Data { get; set; }
        public BasePostbackParam Params { get; set; }
    }
}