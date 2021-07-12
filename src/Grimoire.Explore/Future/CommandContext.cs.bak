using Grimoire.Explore.Abstractions;

namespace Grimoire.Explore
{
    public class CommandContext
    {
        public CommandContext(CommandContext commandContext)
            : this(commandContext.GrimoireContext, commandContext.CommandDescriptor)
        {
        }

        public CommandContext(GrimoireContext grimoireContext, CommandDescriptor commandDescriptor)
        {
            GrimoireContext = grimoireContext;
            CommandDescriptor = commandDescriptor;
            // TODO: deserialize event?
        }

        // public CommandContext(HttpContext httpContext, CommandDescriptor commandDescriptor)
        // {
        //     HttpContext = httpContext;
        //     CommandDescriptor = commandDescriptor;
        //     // TODO: deserialize event?
        // }

        protected CommandContext()
        {
        }

        public GrimoireContext GrimoireContext { get; set; }
        public CommandDescriptor CommandDescriptor { get; set; }
    }
}