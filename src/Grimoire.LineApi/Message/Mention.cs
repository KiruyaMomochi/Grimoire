using System.Collections.Generic;

namespace Grimoire.LineApi.Message
{
    public record Mention
    {
        public List<Mentionee> Mentionees { get; set; }
    }
}