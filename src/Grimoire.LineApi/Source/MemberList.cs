using System.Collections.Generic;
using Grimoire.LineApi.Source;
using Grimoire.LineApi.Things;

namespace Grimoire.LineApi.Member
{
    public record MemberList
    {
        public List<UserSource> Members { get; set; }
    }
}