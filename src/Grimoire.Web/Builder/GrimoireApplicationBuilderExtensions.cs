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
            manager.CollectInvokers(builder.ApplicationServices);
            return builder;
        }
    }
}