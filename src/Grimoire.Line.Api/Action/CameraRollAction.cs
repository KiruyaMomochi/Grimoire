namespace Grimoire.Line.Api.Action
{
    public record CameraRollAction : BaseAction
    {
        public CameraRollAction()
        {
            ActionType = ActionType.CameraRoll;
        }
        public string Label { get; set; }
    }
}