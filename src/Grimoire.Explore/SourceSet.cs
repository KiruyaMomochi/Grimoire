using System;

namespace Grimoire.Explore
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