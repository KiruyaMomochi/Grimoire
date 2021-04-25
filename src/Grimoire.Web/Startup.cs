using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grimoire.Web.Builder;
using Grimoire.Web.Models;
using Grimoire.Web.Services;
using isRock.LineBot;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Grimoire.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("PostgreSQL");
            
            services.AddControllers();
            // services.AddDbContext<GrimoireContext>(
            //     options => 
            //         options
            //             .UseNpgsql(connectionString));

            services.AddDbContext<GrimoireContext>(options => options.UseInMemoryDatabase("grimoire"));
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Grimoire.Web", Version = "v1"});
            });

            services.AddSingleton<UsernameService>();
            services.Configure<LineBotOptions>(Configuration.GetSection(LineBotOptions.LineBot));
            
            services.AddSingleton<IBotService, MockBotService>();
            services.AddGrimoire();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<LineBotOptions> config)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Grimoire.Web v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseGrimoire().UseBot();
            
            // if (config.Value.WebHook != null)
            // {
            //     Console.WriteLine($"Setting webhook to {config.Value.WebHook}");
            //     isRock.LineBot.Utility.SetWebhookEndpointURL(config.Value.ChannelAccessToken, config.Value.WebHook);
            // }
        }
    }
}