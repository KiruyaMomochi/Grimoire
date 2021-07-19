using Grimoire.Explore.CommandRouting;
using Grimoire.Explore.Extensions;
using Grimoire.Line.Api.Webhook.Event;
using Grimoire.Line.Api.Webhook.Message;
using Grimoire.Line.Api.Webhook.Source;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

#nullable enable
namespace Grimoire.Explore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddGrimoire();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });

            //});
            //app.Map("/LineHook", builder => builder.UseMiddleware<LineEndpoint>());

            app.UseGrimoirePackages();
        }
    }
}
#nullable restore