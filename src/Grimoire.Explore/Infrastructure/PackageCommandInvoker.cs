using System;
using System.Linq;
using System.Threading.Tasks;
using Grimoire.Explore.CommandRouting;
using Grimoire.Explore.Common;
using Grimoire.Explore.Package;
using Grimoire.Line.Api.Message;
using Microsoft.Extensions.DependencyInjection;

namespace Grimoire.Explore.Infrastructure
{
#nullable enable
    public class PackageCommandInvoker
    {
        // private readonly PackageContext _packageContext;
        private readonly IServiceProvider _provider;

        public PackageCommandInvoker(PackageCommandDescriptor commandDescriptor, IServiceProvider provider)
        {
            _commandDescriptor = commandDescriptor;
            _provider = provider;
        }

        // internal CommandContext PackageContext => _packageContext;
        private readonly PackageCommandDescriptor _commandDescriptor;

        // public delegate Task<BaseMessage?> PackageContextDelegate(GrimoireContext packageContext);

        public delegate Task<BaseMessage?> ConvertResultDelegate(object? result);

        public ConvertResultDelegate CreateConvertDelegate(Type returnType)
        {
            if (returnType == typeof(string))
                return result => Task.FromResult<BaseMessage?>(new TextMessage()
                {
                    Text = (string?) result
                });

            if (returnType == typeof(Task<string>))
                return async result => new TextMessage()
                {
                    Text = await (Task<string>) result!
                };

            if (returnType.IsAssignableTo(typeof(BaseMessage)))
                return result => Task.FromResult((BaseMessage?) result)!;

            if (returnType.IsAssignableTo(typeof(Task<BaseMessage>)))
                return result => (Task<BaseMessage?>) result!;
            
            return result => Task.FromResult<BaseMessage?>(null);
        }
        
        public LineMessageDelegate CreateInvokeDelegate()
        {
            var packageType = _commandDescriptor.PackageType;
            var packageMethod = _commandDescriptor.Method;
            var len = _commandDescriptor.Parameters.Count;
            var isInt = _commandDescriptor.Parameters.Select(x => x.ParameterType == typeof(int)).ToArray();
            var factory = ActivatorUtilities.CreateFactory(packageType, Array.Empty<Type>());
            var contextSetter =
                PropertyHelper.MakeFastPropertySetter<PackageBase>(
                    typeof(PackageBase).GetProperty(nameof(PackageBase.GrimoireContext)));
            var convertDelegate = CreateConvertDelegate(packageMethod.ReturnType);

            return context =>
            {
                var parameters = new object[len];
                var splitArgs = context.Args.Split((char[]?) null, len + 1,
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < len; i++)
                {
                    if (isInt[i])
                    {
                        if (int.TryParse(splitArgs[i], out var res))
                            parameters[i] = res;
                        else
                            return Task.FromResult<BaseMessage?>(new TextMessage
                            {
                                Text = $"Failed to parse {splitArgs[i]} to int. Please check your input."
                            });
                    }
                    else
                        parameters[i] = splitArgs[i];
                }

                context.Args = splitArgs[^1];

                var packageInstance = factory.Invoke(_provider, null);
                if (packageInstance is PackageBase packageBase)
                    contextSetter(packageBase, context);
                var result = packageMethod.Invoke(packageInstance, parameters);
                return convertDelegate(result);
            };
        }
    }
#nullable restore
}