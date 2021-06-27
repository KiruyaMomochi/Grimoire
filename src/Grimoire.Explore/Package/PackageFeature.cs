using System.Collections.Generic;
using System.Reflection;

namespace Grimoire.Explore.Package
{
    public class PackageFeature
    {
        public IList<TypeInfo> Packages { get; } = new List<TypeInfo>();
    }
}