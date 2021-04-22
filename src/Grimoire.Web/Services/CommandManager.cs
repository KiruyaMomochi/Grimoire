using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Grimoire.LineApi.Event;
using Grimoire.LineApi.Message;
using Grimoire.Web.Builder;
using Grimoire.Web.Commands;
using Grimoire.Web.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Grimoire.Web.Services
{
    public class CommandManager
    {
        public delegate Task CommandInvoker(MessageEvent args, string command);
        
        private readonly Dictionary<string, CommandActivator> _commandActivators = new();
        private readonly Dictionary<string, CommandInvoker> _commandInvokers = new();
        private readonly char _startSymbol = '#';

        public void CollectCommands(IServiceCollection services)
        {
            var commandBase = typeof(CommandBase);
            var assembly = AppDomain.CurrentDomain.GetAssemblies();
            var types = assembly.SelectMany(a => a.GetTypes()).Where(
                t => commandBase.IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var type in types)
            {
                var methods = type.GetMethods();
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes<GroupCommandAttribute>();
                    foreach (var attribute in attributes)
                    {
                        var commands = attribute.GetCommands();
                        foreach (var command in commands)
                        {
                            var prop = type.GetProperty("CommandContext", BindingFlags.Public | BindingFlags.Instance);
                            var setter = PropertyHelper.MakeFastPropertySetter<CommandBase>(prop);
                            var activator = ActivatorUtilities.CreateFactory(type, Type.EmptyTypes);

                            _commandActivators.Add(command,
                                new CommandActivator
                                    {Activator = activator, Method = method, SetCommandContext = setter, Type = type});
                        }

                        services.AddScoped(type);
                    }
                }
            }
        }

        public void CollectInvokers(IServiceProvider provider)
        {
            foreach (var (command, value) in _commandActivators)
            {
                _commandInvokers.Add(command, async (e, a) =>
                {
                    using var scope = provider.CreateScope();
                    var service = value.Activator(scope.ServiceProvider, null) as CommandBase;
                    value.SetCommandContext(service, new CommandContext {Event = e, Args = a});
                    var ret = value.Method.Invoke(service, null);
                    if (ret is Task task) await task;
                });
            }
        }

        private (string command, string args) SplitText(string text)
        {
            var tokens = text.Split((char[]) null, 2,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return tokens.Length switch
            {
                0 => (null, null),
                1 => (tokens[0], null),
                2 => (tokens[0], tokens[1]),
                _ => throw new IndexOutOfRangeException()
            };
        }

        public async Task HandleWebhookEvent(BaseEvent e)
        {
            if (e is not MessageEvent {Message: TextMessage textMessage} messageEvent) return;
            if (!textMessage.Text.StartsWith(_startSymbol)) 
                return;

            var (command, args) = SplitText(textMessage.Text[1..]);
            if (_commandInvokers.TryGetValue(command, out var invoker))
                await invoker(messageEvent, args);
        }
    }
}