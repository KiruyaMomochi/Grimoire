using System.Collections.Generic;
using System.Reflection;
using Grimoire.Explore.Abstractions;

namespace Grimoire.Explore.Package
{
    public class PackageCommandDescriptor : CommandDescriptor
    {
        public string PackageName { get; init; }

        public TypeInfo PackageType { get; init; }
        public MethodInfo Method { get; init; }

        // CacheEntry
        
    }
}