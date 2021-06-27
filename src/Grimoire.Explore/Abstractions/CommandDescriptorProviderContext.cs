using System.Collections.Generic;

namespace Grimoire.Explore.Abstractions
{
    public class CommandDescriptorProviderContext
    {
        public IList<CommandDescriptor> Results { get; } = new List<CommandDescriptor>();
    }
}