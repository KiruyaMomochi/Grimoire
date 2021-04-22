namespace Grimoire.LineApi.Things
{
    public record Scenario : BaseThings
    {
        public string DeviceId { get; set; }
        public ScenarioResult ScenarioResult { get; set; }
    }
}