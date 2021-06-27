using System.Collections.Generic;

namespace Grimoire.Line.Api.Webhook.Source
{
    public record MemberList
    {
        public List<UserSource> Members { get; set; }
    }
}