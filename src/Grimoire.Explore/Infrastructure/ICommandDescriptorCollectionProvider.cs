using System.Collections.Generic;
using Grimoire.Explore.Abstractions;

namespace Grimoire.Explore.Infrastructure
{
    public interface ICommandDescriptorCollectionProvider
    {
        IReadOnlyList<CommandDescriptor> CommandDescriptors { get; }
    }
}