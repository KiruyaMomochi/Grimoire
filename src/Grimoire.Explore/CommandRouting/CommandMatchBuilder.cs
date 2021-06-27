#nullable enable
using System;
using System.Collections.Generic;
using Grimoire.Explore.Abstractions;
using Grimoire.Explore.Infrastructure;
using Grimoire.Explore.Package;

namespace Grimoire.Explore.CommandRouting
{
    public class CommandMatchBuilder
    {
        public CommandMatchBuilder(IServiceProvider provider)
        {
            _provider = provider;
        }

        private readonly Dictionary<string, List<CommandMatchEntry>> _commandEndpointDict = new();
        private readonly IServiceProvider _provider;

        public void Add(CommandDescriptor commandDescriptor)
        {
            if (commandDescriptor is not PackageCommandDescriptor descriptor)
                throw new NotSupportedException();
            var invoker = new PackageCommandInvoker(descriptor, _provider).CreateInvokeDelegate();

            if (!_commandEndpointDict.TryGetValue(commandDescriptor.Command, out var endpoints))
            {
                endpoints = new List<CommandMatchEntry>();
                _commandEndpointDict[commandDescriptor.Command] = endpoints;
            }
            
            endpoints.Add(new CommandMatchEntry(commandDescriptor, invoker));
        }

        public Dictionary<string, List<CommandMatchEntry>> Build()
        {
            return _commandEndpointDict;
        }
    }
}