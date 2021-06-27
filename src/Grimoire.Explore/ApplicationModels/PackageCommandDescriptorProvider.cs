using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Grimoire.Explore.Abstractions;
using Grimoire.Explore.Package;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Grimoire.Explore.ApplicationModels
{
    public class PackageCommandDescriptorProvider : ICommandDescriptorProvider
    {
        private readonly ApplicationPartManager _partManager;

        public PackageCommandDescriptorProvider(ApplicationPartManager partManager)
        {
            _partManager = partManager;
        }

        public void OnProvidersExecuting(CommandDescriptorProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            foreach (var descriptor in GetDescriptors())
            {
                context.Results.Add(descriptor);
            }
        }

        public void OnProvidersExecuted(CommandDescriptorProviderContext context)
        {
            // Do nothing
        }

        internal IEnumerable<PackageCommandDescriptor> GetDescriptors()
        {
            var packageTypes = GetPackageTypes();
            return Flatten(packageTypes, CreateCommandDescriptor);
        }

        private static List<TResult> Flatten<TResult>(IEnumerable<TypeInfo> packageTypes, Func<TypeInfo, MethodInfo, CommandAttribute, string, TResult> flattener)
        {
            var results = new List<TResult>();

            foreach (var packageType in packageTypes)
            {
                var methods = packageType.GetMethods();
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes<CommandAttribute>(true);
                    foreach (var attribute in attributes)
                    {
                        var commands = attribute.Commands;
                        foreach (var command in commands)
                        {
                            var result = flattener(packageType, method, attribute, command);
                            Debug.Assert(result != null);
                            results.Add(result);
                        }
                    }
                }
            }

            return results;
        }

        private static PackageCommandDescriptor CreateCommandDescriptor(TypeInfo packageType, MethodInfo commandMethod,
            CommandAttribute attribute, string command)
        {
            var commandName = commandMethod.Name;
            if (commandName.EndsWith("Command"))
                commandName = commandName[..^"Command".Length];
            var commandParameters = commandMethod.GetParameters();
            
            return new PackageCommandDescriptor
            {
                DisplayName = commandName,
                Method = commandMethod,
                PackageType = packageType,
                PackageName = packageType.FullName,
                Command = command,
                Parameters = commandParameters,
                SourceSet = attribute.SourceSet
            };
        }

        private IEnumerable<TypeInfo> GetPackageTypes()
        {
            var feature = new PackageFeature();
            _partManager.PopulateFeature(feature);

            return feature.Packages;
        }

        public int Order => -1000;
    }
}