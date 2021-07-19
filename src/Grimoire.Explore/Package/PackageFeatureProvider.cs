using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grimoire.Explore.Attributes;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Grimoire.Explore.Package
{
    public class PackageFeatureProvider : IApplicationFeatureProvider<PackageFeature>
    {
        private const string PackageTypeNameSuffix = "Package";

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, PackageFeature feature)
        {
            foreach (var part in parts.OfType<IApplicationPartTypeProvider>())
            {
                foreach (var type in part.Types)
                {
                    if (IsPackage(type) && !feature.Packages.Contains(type))
                    {
                        feature.Packages.Add(type);
                    }
                }
            }
        }

        protected virtual bool IsPackage(TypeInfo typeInfo)
        {
            if (!typeInfo.IsClass)
                return false;
            if (typeInfo.IsAbstract)
                return false;
            if (!typeInfo.IsPublic)
                return false;
            if (typeInfo.ContainsGenericParameters)
                return false;
            if (typeInfo.IsDefined(typeof(NonPackageAttribute)))
                return false;
            return typeInfo.Name.EndsWith(PackageTypeNameSuffix, StringComparison.OrdinalIgnoreCase) ||
                   typeInfo.IsDefined(typeof(PackageAttribute));
        }
    }
}