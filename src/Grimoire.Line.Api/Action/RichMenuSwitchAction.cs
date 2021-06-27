namespace Grimoire.Line.Api.Action
{
    public record RichMenuSwitchAction : BaseAction
    {
        public RichMenuSwitchAction()
        {
            ActionType = ActionType.Richmenuswitch;
        }
        public string RichMenuAliasId { get; set; }
        public string Data { get; set; }
    }
}