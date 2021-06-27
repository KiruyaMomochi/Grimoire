using System.Reflection;
using Grimoire.Explore.Abstractions;

namespace Grimoire.Explore.Package
{
    public class PackageCommandDescriptor : CommandDescriptor
    {
        public string PackageName { get; set; }

        public TypeInfo PackageType { get; set; }
        public MethodInfo Method { get; set; }

        // CacheEntry
        
    }
}