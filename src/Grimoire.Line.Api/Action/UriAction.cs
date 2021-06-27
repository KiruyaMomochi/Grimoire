namespace Grimoire.Line.Api.Action
{
    public record UriAction : BaseAction
    {
        public UriAction() { ActionType = ActionType.Uri; }
        public string Label { get; set; }
        public string Uri { get; set; }
        public Uri.AltUri AltUri { get; set; }
    }
}