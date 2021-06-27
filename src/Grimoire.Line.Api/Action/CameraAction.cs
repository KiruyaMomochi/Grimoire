namespace Grimoire.Line.Api.Action
{
    public record CameraAction : BaseAction
    {
        public CameraAction()
        {
            ActionType = ActionType.Camera;
        }
        public string Label { get; set; }
    }
}