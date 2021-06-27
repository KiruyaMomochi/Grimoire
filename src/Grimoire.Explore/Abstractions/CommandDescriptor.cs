using System.Collections.Generic;
using System.Reflection;

namespace Grimoire.Explore.Abstractions
{
    public abstract class CommandDescriptor
    {
        public string Command { get; set; }
        public SourceSet SourceSet { get; set; }
        public string DisplayName { get; set; }
        
        public IList<ParameterInfo> Parameters { get; set; }
    }

}
