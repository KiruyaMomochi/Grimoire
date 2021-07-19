using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grimoire.Explore.Abstractions;
using Grimoire.Explore.CommandRouting;
using Grimoire.Explore.Common;
using Grimoire.Explore.Parameter;
using Grimoire.Line.Api.Message;
using Microsoft.Extensions.DependencyInjection;

namespace Grimoire.Explore.Package
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
                    Text = (string?)result
                });

            if (returnType == typeof(Task<string>))
                return async result => new TextMessage()
                {
                    Text = await (Task<string>)result!
                };

            if (returnType == typeof(StringBuilder))
                return result => Task.FromResult<BaseMessage?>(new TextMessage()
                {
                    Text = ((StringBuilder)result!).ToString().Trim()
                });

            if (returnType == typeof(Task<StringBuilder>))
                return async result => new TextMessage()
                {
                    Text = (await (Task<StringBuilder>)result!).ToString().Trim()
                };

            if (returnType.IsAssignableTo(typeof(BaseMessage)))
                return result => Task.FromResult((BaseMessage?)result)!;

            if (returnType.IsAssignableTo(typeof(Task<BaseMessage>)))
                return result => (Task<BaseMessage?>)result!;

            return _ => Task.FromResult<BaseMessage?>(null);
        }

        public LineMessageDelegate CreateInvokeDelegate()
        {
            var packageType = _commandDescriptor.PackageType;
            var packageMethod = _commandDescriptor.Method;
            var len = _commandDescriptor.Parameters.Count;
            var type = _commandDescriptor.Parameters.Select(x =>
            {
                if (x.ParameterType == typeof(int)) return ParameterType.Signed;
                if (x.ParameterType == typeof(uint)) return ParameterType.Unsigned;
                return ParameterType.String;
            }).ToArray();

            var factory = ActivatorUtilities.CreateFactory(packageType, Array.Empty<Type>());
            var contextSetter =
                PropertyHelper.MakeFastPropertySetter<PackageBase>(
                    typeof(PackageBase).GetProperty(nameof(PackageBase.GrimoireContext)));
            var convertDelegate = CreateConvertDelegate(packageMethod.ReturnType);
            var initMethod = packageType.GetDeclaredMethod(nameof(PackageBase.OnInitializedAsync));

            return context =>
            {
                var packageInstance = factory.Invoke(_provider.CreateScope().ServiceProvider, null);
                if (packageInstance is PackageBase packageBase)
                    contextSetter(packageBase, context);
                initMethod?.Invoke(packageInstance, null);
                var result = packageMethod.Invoke(packageInstance, context.RealArgs);
                return convertDelegate(result);
            };
        }
    }
#nullable restore
}