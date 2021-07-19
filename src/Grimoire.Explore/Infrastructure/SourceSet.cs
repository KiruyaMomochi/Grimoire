using System;

namespace Grimoire.Explore.Infrastructure
{
    [Flags]
    public enum SourceSet
    {
        None,
        User,
        Group,
        Room
    }
}