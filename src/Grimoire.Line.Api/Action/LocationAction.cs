namespace Grimoire.Line.Api.Action
{
    public record LocationAction : BaseAction
    {
        public LocationAction()
        {
            ActionType = ActionType.Location;
        }
        public string Label { get; set; }
    }
}