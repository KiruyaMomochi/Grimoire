using System.Collections.Generic;

namespace Grimoire.Explore.Abstractions
{
    public interface ICommandDescriptorCollectionProvider
    {
        IReadOnlyList<CommandDescriptor> CommandDescriptors { get; }
    }
}