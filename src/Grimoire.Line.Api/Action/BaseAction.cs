using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Grimoire.Line.Api.Action
{
    public abstract record BaseAction
    {
        [JsonPropertyName("type")] public ActionType ActionType { get; protected set; }
    }
}