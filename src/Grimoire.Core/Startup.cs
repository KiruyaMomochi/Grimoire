using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grimoire.Core.Services;
using Grimoire.Data;
using Grimoire.Data.Models;
using Grimoire.Explore;
using Grimoire.Explore.CommandRouting;
using Grimoire.Explore.Extensions;
using Grimoire.Line.Api.Webhook.Event;
using Grimoire.Line.Api.Webhook.Message;
using Grimoire.Line.Api.Webhook.Source;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Grimoire.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("PostgreSQL");

            services.AddDbContext<GrimoireDatabaseContext>(
                options =>
                    options
                        .UseInMemoryDatabase("grimoire"));
                        // .UseNpgsql(connectionString));

            services.AddControllers();
            services.AddGrimoire();
            services.AddSingleton<IBotService, MockBotService>();
            services.AddSingleton<LineSignatureService>();
            services.AddSingleton<UsernameService>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Grimoire.Core", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Grimoire.Core v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseGrimoirePackages();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.ApplicationServices.GetRequiredService<CommandManager>().HandleWebhookEvent(new MessageEvent()
            {
                Message = new TextMessage()
                {
                    Text = "#raw 233 test qwq adjfojdfsao"
                },
                Source = new UserSource()
            }).Wait();
            app.ApplicationServices.GetRequiredService<CommandManager>().HandleWebhookEvent(new MessageEvent()
            {
                Message = new TextMessage()
                {
                    Text = "#raw 2333333333333333 123 ..."
                },
                Source = new UserSource()
            }).Wait();
        }
    }
}