namespace Grimoire.Line.Api.Message.ImageMap
{
    public record UriAction : BaseAction
    {
        public UriAction() { ActionType = ActionType.Uri; }
        public string Label { get; set; }
        public string LinkUri { get; set; }
        public Area Area { get; set; }
    }
}