namespace Grimoire.Line.Api.Action
{
    public record PostbackAction : BaseAction
    {
        public PostbackAction() { ActionType = ActionType.Postback; }
        public string Label { get; set; }

        public string Data { get; set; }
        public string DisplayText { get; set; }
        public string Text { get; set; }
    }
}