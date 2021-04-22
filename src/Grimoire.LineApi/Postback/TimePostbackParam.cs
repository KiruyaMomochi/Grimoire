using System;

namespace Grimoire.LineApi.Postback
{
    public record TimePostbackParam : BasePostbackParam
    {
        public string Time { get; set; }
    }
}