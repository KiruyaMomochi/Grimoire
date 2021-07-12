using System;
using Grimoire.Explore.Package;

namespace Grimoire.Explore
{
    public class PackageContext : CommandContext
    {
        public PackageContext(CommandContext commandContext) : base(commandContext)
        {
            if (commandContext.CommandDescriptor is not PackageCommandDescriptor)
                throw new ArgumentException(null, nameof(commandContext));
        }

        public PackageContext() : base()
        {
        }

        public new PackageCommandDescriptor CommandDescriptor
        {
            get => (PackageCommandDescriptor) base.CommandDescriptor;
            set => base.CommandDescriptor = value;
        }
    }
}