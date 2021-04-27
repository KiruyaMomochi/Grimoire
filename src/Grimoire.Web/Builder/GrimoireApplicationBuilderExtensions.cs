using Grimoire.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Grimoire.Web.Builder
{
    public static class GrimoireApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGrimoire(this IApplicationBuilder builder)
        {
            var manager = builder.ApplicationServices.GetRequiredService<CommandManager>();
            manager.UseLogging(builder.ApplicationServices);
            manager.CollectInvokers(builder.ApplicationServices);
            return builder;
        }
        
        public static IApplicationBuilder UseBot(this IApplicationBuilder builder)
        {
            var manager = builder.ApplicationServices.GetRequiredService<CommandManager>();
            var bot = builder.ApplicationServices.GetRequiredService<IBotService>();
            manager.Bot = bot;
            return builder;
        }
    }
}