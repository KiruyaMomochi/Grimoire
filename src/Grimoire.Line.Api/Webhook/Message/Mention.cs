using System.Collections.Generic;

namespace Grimoire.Line.Api.Webhook.Message
{
    public record Mention
    {
        public List<Mentionee> Mentionees { get; set; }
    }
}