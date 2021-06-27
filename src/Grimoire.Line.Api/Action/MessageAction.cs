namespace Grimoire.Line.Api.Action
{
    public record MessageAction : BaseAction
    {
        public MessageAction() { ActionType = ActionType.Message; }
        public string Label { get; set; }
        public string Text { get; set; }
    }
}