using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using Grimoire.Explore.Abstractions;
using Grimoire.Explore.ApplicationModels;
using Grimoire.Explore.Infrastructure;
using Grimoire.Explore.Package;
using Grimoire.Line.Api.Message;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Hosting;

namespace Grimoire.Explore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        public static void DumpAllPackagesFromCurrentDomain()
        {
            var applicationManager = ManagerWithAssemblies();
            var features = new PackageFeature();
            applicationManager.PopulateFeature(features);

            foreach (var system in features.Packages)
            {
                Console.WriteLine(system.FullName);
            }
        }

        public static void TestMessage()
        {
            var textMessage = new TextMessage()
            {
                Text = "233",
                Sender = new Sender()
                {
                    Name = "a",
                    IconUrl = "b"
                }
            };
            Console.WriteLine(JsonSerializer.Serialize(textMessage, Options.SerializerOption));
        }

        public static void TestInvoke()
        {
            var provider = new PackageCommandDescriptorProvider(ManagerWithAssemblies());
            var collectionProvider = new DefaultCommandDescriptorCollectionProvider(new ICommandDescriptorProvider[] {provider});
            // var commandInvokerFactory = new CommandInvokerFactory(new ICommandInvokerProvider[]{new PackageCommandInvokerProvider()});

            foreach (var descriptor in collectionProvider.CommandDescriptors)
            {
                var grimoireContext = new GrimoireContext() { Command = descriptor.Command };
                var commandContext = new CommandContext(grimoireContext, descriptor);
                // var invoker = commandInvokerFactory.CreateInvoker(commandContext);
                // invoker.InvokeAsync();
            }
        }

        public static ApplicationPartManager ManagerWithAssemblies()
        {
            var applicationPartManager = new ApplicationPartManager();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var seenAssemblies = new HashSet<Assembly>();
            foreach (var assembly in assemblies)
            {
                if (!seenAssemblies.Add(assembly))
                    continue;

                var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
                foreach (var applicationPart in partFactory.GetApplicationParts(assembly))
                {
                    applicationPartManager.ApplicationParts.Add(applicationPart);
                }
            }

            applicationPartManager.FeatureProviders.Add(new PackageFeatureProvider());
            return applicationPartManager;
        }

        public static void DumpPackageActionDescriptorProvider()
        {
            var packageActionDescriptorProvider = new PackageCommandDescriptorProvider(ManagerWithAssemblies());
            // var controllerTypes = packageActionDescriptorProvider.GetControllerTypes();
            // foreach (var controller in controllerTypes)
            // {
            // Console.WriteLine(controller.FullName);
            // }
        }
    }
}