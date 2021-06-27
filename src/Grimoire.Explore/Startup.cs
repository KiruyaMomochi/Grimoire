using Grimoire.Explore.CommandRouting;
using Grimoire.Line.Api.Webhook.Event;
using Grimoire.Line.Api.Webhook.Message;
using Grimoire.Line.Api.Webhook.Source;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Grimoire.Explore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddGrimoirePackages();
            services.TryAddSingleton<CommandMatchBuilder>();
            services.TryAddSingleton<CommandManager>();
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

            var commandManager = app.ApplicationServices.GetRequiredService<CommandManager>();
            commandManager.Collect();
            commandManager.HandleWebhookEvent(new MessageEvent()
            {
                Message = new TextMessage()
                {
                    Text = "#ping test qwq adjfojdfsao"
                },
                Source = new UserSource()
            }).Wait();
        }
    }
}