using Grimoire.Explore.CommandRouting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

#nullable enable
namespace Grimoire.Explore
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGrimoirePackages(this IApplicationBuilder builder)
        {
            var commandManager = builder.ApplicationServices.GetRequiredService<CommandManager>();
            commandManager.Collect();

            return builder;
        }
    }
}
#nullable restore
