using Grimoire.Line.Api.Webhook.Event;

namespace Grimoire.Explore.Infrastructure
{
    public class GrimoireContext
    {
        public string Command { get; set; }
        public object[] RealArgs { get; set; }
        public string Args { get; set; }
        public BaseEvent Event { get; set; }
    }
}
