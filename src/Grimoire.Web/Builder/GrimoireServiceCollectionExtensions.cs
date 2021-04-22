using Grimoire.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Grimoire.Web.Builder
{
    public static class GrimoireServiceCollectionExtensions
    {
        public static IServiceCollection AddGrimoire(this IServiceCollection services)
        {
            var manager = new CommandManager();
            services.AddSingleton(manager);
            manager.CollectCommands(services);
            return services;
        }
    }
}