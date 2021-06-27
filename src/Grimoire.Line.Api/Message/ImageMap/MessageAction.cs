namespace Grimoire.Line.Api.Message.ImageMap
{
    public record MessageAction : BaseAction
    {
        public MessageAction() { ActionType = ActionType.Message; }
        public string Label { get; set; }
        public string Text { get; set; }
        public Area Area { get; set; }
    }
}