using System;
using System.Linq;
using System.Text;
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

        private static ConvertResultDelegate CreateConvertDelegate(Type returnType)
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

            if (returnType == typeof(StringBuilder))
                return result => Task.FromResult<BaseMessage?>(new TextMessage()
                {
                    Text = ((StringBuilder) result!).ToString().Trim()
                });

            if (returnType == typeof(Task<StringBuilder>))
                return async result => new TextMessage()
                {
                    Text = (await (Task<StringBuilder>) result!).ToString().Trim()
                };
            
            if (returnType.IsAssignableTo(typeof(BaseMessage)))
                return result => Task.FromResult((BaseMessage?) result)!;

            if (returnType.IsAssignableTo(typeof(Task<BaseMessage>)))
                return result => (Task<BaseMessage?>) result!;

            return _ => Task.FromResult<BaseMessage?>(null);
        }

        private enum VariableType
        {
            Int,
            Uint,
            Unknown
        }

        public LineMessageDelegate CreateInvokeDelegate()
        {
            var packageType = _commandDescriptor.PackageType;
            var packageMethod = _commandDescriptor.Method;
            var len = _commandDescriptor.Parameters.Count;
            var type = _commandDescriptor.Parameters.Select(x =>
            {
                if (x.ParameterType == typeof(int)) return VariableType.Int;
                if (x.ParameterType == typeof(uint)) return VariableType.Uint;
                return VariableType.Unknown;
            }).ToArray();

            var factory = ActivatorUtilities.CreateFactory(packageType, Array.Empty<Type>());
            var contextSetter =
                PropertyHelper.MakeFastPropertySetter<PackageBase>(
                    typeof(PackageBase).GetProperty(nameof(PackageBase.GrimoireContext)));
            var convertDelegate = CreateConvertDelegate(packageMethod.ReturnType);
            var initMethod = packageType.GetDeclaredMethod(nameof(PackageBase.OnInitializedAsync));

            return context =>
            {
                var parameters = new object[len];
                var splitArgs = context.Args.Split((char[]?) null, len + 1,
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                if (splitArgs.Length < len)
                    return Task.FromResult<BaseMessage?>(new TextMessage()
                    {
                        Text = "Not enough arguments."
                    });

                for (var i = 0; i < len; i++)
                {
                    switch (type[i])
                    {
                        case VariableType.Int:
                            if (int.TryParse(splitArgs[i], out var intRes))
                                parameters[i] = intRes;
                            else
                                return Task.FromResult<BaseMessage?>(new TextMessage
                                {
                                    Text = $"Failed to parse {splitArgs[i]} to int. Please check your input."
                                });
                            break;
                        case VariableType.Uint:
                            if (uint.TryParse(splitArgs[i], out var uintRes))
                                parameters[i] = uintRes;
                            else
                                return Task.FromResult<BaseMessage?>(new TextMessage
                                {
                                    Text = $"Failed to parse {splitArgs[i]} to int. Please check your input."
                                });
                            break;
                        case VariableType.Unknown:
                            parameters[i] = splitArgs[i];
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                context.Args = splitArgs.Length == len ? null : splitArgs[^1];

                var packageInstance = factory.Invoke(_provider.CreateScope().ServiceProvider, null);
                if (packageInstance is PackageBase packageBase)
                    contextSetter(packageBase, context);
                initMethod?.Invoke(packageInstance, null);
                var result = packageMethod.Invoke(packageInstance, parameters);
                return convertDelegate(result);
            };
        }
    }
#nullable restore
}