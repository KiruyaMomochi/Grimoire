#nullable enable
using System;
using System.Linq;
using Grimoire.Explore.Abstractions;
using Grimoire.Explore.ApplicationModels;
using Grimoire.Explore.CommandRouting;
using Grimoire.Explore.Infrastructure;
using Grimoire.Explore.Package;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

#nullable enable
namespace Grimoire.Explore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGrimoire(this IServiceCollection services)
        {
            services.AddGrimoirePackages();
            services.TryAddSingleton<CommandMatchBuilder>();
            services.TryAddSingleton<CommandManager>();
            
            return services;
        }
    
        public static IServiceCollection AddGrimoirePackages(this IServiceCollection services)
        {
            var partManager = GetApplicationPartManager(services);
            services.TryAddSingleton(partManager);
            ConfigureDefaultFeatureProviders(partManager);
            
            // services.TryAddEnumerable(ServiceDescriptor
                // .Transient<ICommandInvokerProvider, PackageCommandInvokerProvider>());
            
            services.TryAddEnumerable(ServiceDescriptor
                .Transient<ICommandDescriptorProvider, PackageCommandDescriptorProvider>());
            
            // services.TryAddSingleton<ICommandInvokerFactory, CommandInvokerFactory>();
            services.TryAddSingleton<ICommandDescriptorCollectionProvider, DefaultCommandDescriptorCollectionProvider>();
            
            return services;
        }

        private static void ConfigureDefaultFeatureProviders(ApplicationPartManager partManager)
        {
            if (!partManager.FeatureProviders.OfType<PackageFeatureProvider>().Any())
                partManager.FeatureProviders.Add(new PackageFeatureProvider());
        }

        private static ApplicationPartManager GetApplicationPartManager(IServiceCollection services)
        {
            var manager = GetServiceFromCollection<ApplicationPartManager>(services);
            if (manager == null)
                throw new NullReferenceException(nameof(manager));
            
            return manager;
        }

        
        private static T? GetServiceFromCollection<T>(IServiceCollection services)
        {
            return (T?)services
                .LastOrDefault(d => d.ServiceType == typeof(T))
                ?.ImplementationInstance;
        }
    }
}
#nullable restore
