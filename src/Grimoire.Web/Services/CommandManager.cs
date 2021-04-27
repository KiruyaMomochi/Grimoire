using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Grimoire.LineApi.Event;
using Grimoire.LineApi.Message;
using Grimoire.LineApi.Source;
using Grimoire.Web.Builder;
using Grimoire.Web.Commands;
using Grimoire.Web.Common;
using Grimoire.Web.Replies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Grimoire.Web.Services
{
    public class CommandManager
    {
        public delegate Task CommandInvoker(MessageEvent args, string command);

        private readonly Dictionary<string, CommandActivator> _groupCommandActivators = new();
        private readonly Dictionary<string, CommandInvoker> _groupCommandInvokers = new();
        private readonly Dictionary<string, CommandActivator> _userCommandActivators = new();
        private readonly Dictionary<string, CommandInvoker> _userCommandInvokers = new();
        private readonly Dictionary<string, CommandActivator> _roomCommandActivators = new();
        private readonly Dictionary<string, CommandInvoker> _roomCommandInvokers = new();
        private readonly char _startSymbol = '#';
        private ILogger<CommandManager> _logger;
        public IBotService Bot { get; set; }

        /// <summary>
        /// Collect all commands.
        /// A method is a command if it's class inherits from CommandBase
        /// and the method itself has GroupCommand attribute.
        /// </summary>
        /// <param name="services"></param>
        public void CollectCommands(IServiceCollection services)
        {
            // Find all class that is assignable from commandBase
            var commandBase = typeof(CommandBase);
            var assembly = AppDomain.CurrentDomain.GetAssemblies();
            var types = assembly.SelectMany(a => a.GetTypes()).Where(
                t => commandBase.IsAssignableFrom(t) && !t.IsAbstract);

            // Add each command activators to the _commandActivators dictionary
            foreach (var type in types)
            {
                var methods = type.GetMethods();
                foreach (var method in methods)
                {
                    CollectCommandAttributes(services, method, type);
                }
            }
        }

        private void CollectCommandAttributes(IServiceCollection services, MethodInfo method, Type type)
        {
            var attributes = method.GetCustomAttributes<CommandAttribute>(true);
            foreach (var attribute in attributes)
            {
                var commands = attribute.Commands;

                if ((attribute.SourceSet & SourceSet.Group) == SourceSet.Group)
                    foreach (var command in commands)
                        AddCommandToActivators(type, command, method, _groupCommandActivators);
                if ((attribute.SourceSet & SourceSet.User) == SourceSet.User)
                    foreach (var command in commands)
                        AddCommandToActivators(type, command, method, _userCommandActivators);
                if ((attribute.SourceSet & SourceSet.Room) == SourceSet.Room)
                    foreach (var command in commands)
                        AddCommandToActivators(type, command, method, _roomCommandActivators);

                services.AddScoped(type);
            }
        }

        // TODO: use a clear variable name instead of current, which is difficult to understand
        private void AddCommandToActivators(Type type, string command, MethodInfo method,
            Dictionary<string, CommandActivator> activators)
        {
            var prop = type.GetProperty(nameof(CommandContext), BindingFlags.Public | BindingFlags.Instance);
            var setter = PropertyHelper.MakeFastPropertySetter<CommandBase>(prop);
            var factory = ActivatorUtilities.CreateFactory(type, Type.EmptyTypes);
            var initMethod = type.GetMethod(nameof(CommandBase.OnInitializedAsync),
                BindingFlags.Public | BindingFlags.Instance);
            var afterMethod = type.GetMethod(nameof(CommandBase.OnAfterCommandAsync),
                BindingFlags.Public | BindingFlags.Instance);

            var activator = new CommandActivator
            {
                Factory = factory,
                Method = method,
                SetCommandContext = setter,
                Type = type
            };

            if (initMethod != null)
                activator.OnInitializedAsync = (service) => initMethod.Invoke(service, null) as Task;
            if (afterMethod != null)
                activator.OnAfterCommandAsync = (service) => afterMethod.Invoke(service, null) as Task;
            activators.Add(command, activator);
        }

        public void CollectInvokers(IServiceProvider provider)
        {
            CollectInvokers(provider, _groupCommandActivators, _groupCommandInvokers);
            CollectInvokers(provider, _userCommandActivators, _userCommandInvokers);
            CollectInvokers(provider, _roomCommandActivators, _roomCommandInvokers);
        }

        // TODO: refactor
        // TODO: use c# argument to pass command arguments
        private void CollectInvokers(IServiceProvider provider, Dictionary<string, CommandActivator> activators,
            Dictionary<string, CommandInvoker> invokers)
        {
            foreach (var (command, activator) in activators)
            {
                var method = activator.Method;

                if (typeof(TextReply).IsAssignableFrom(method.ReturnType))
                {
                    invokers.Add(command, async (e, a) =>
                    {
                        using var scope = provider.CreateScope();
                        var service = activator.Factory(scope.ServiceProvider, null) as CommandBase;
                        activator.SetCommandContext(service, new CommandContext {Event = e, Args = a});

                        await activator.OnInitializedAsync(service);
                        var ret = (TextReply) activator.Method.Invoke(service, null);
                        Debug.Assert(ret != null, nameof(ret) + " != null");
                        Bot.ReplyMessage(e.ReplyToken, ret.Message);
                        await activator.OnAfterCommandAsync(service);
                    });
                }
                else if (typeof(Task<TextReply>).IsAssignableFrom(method.ReturnType))
                {
                    invokers.Add(command, async (e, a) =>
                    {
                        using var scope = provider.CreateScope();
                        var service = activator.Factory(scope.ServiceProvider, null) as CommandBase;
                        activator.SetCommandContext(service, new CommandContext {Event = e, Args = a});

                        await activator.OnInitializedAsync(service);
                        var ret = (Task<TextReply>) activator.Method.Invoke(service, null);
                        Debug.Assert(ret != null, nameof(ret) + " != null");
                        var textReply = await ret;
                        Bot.ReplyMessage(e.ReplyToken, textReply.Message);
                        await activator.OnAfterCommandAsync(service);
                    });
                }
                else if (typeof(Task).IsAssignableFrom(method.ReturnType))
                {
                    invokers.Add(command, async (e, a) =>
                    {
                        using var scope = provider.CreateScope();
                        var service = activator.Factory(scope.ServiceProvider, null) as CommandBase;
                        activator.SetCommandContext(service, new CommandContext {Event = e, Args = a});

                        await activator.OnInitializedAsync(service);
                        var ret = (Task) activator.Method.Invoke(service, null);
                        Debug.Assert(ret != null, nameof(ret) + " != null");
                        await ret;
                        await activator.OnAfterCommandAsync(service);
                    });
                }
                else
                {
                    invokers.Add(command, async (e, a) =>
                    {
                        using var scope = provider.CreateScope();
                        var service = activator.Factory(scope.ServiceProvider, null) as CommandBase;
                        activator.SetCommandContext(service, new CommandContext {Event = e, Args = a});
                        
                        await activator.OnInitializedAsync(service);
                        var ret = activator.Method.Invoke(service, null);

                        switch (ret)
                        {
                            case Task<TextReply> replyTask:
                                var text = await replyTask;
                                Bot.ReplyMessage(e.ReplyToken, text.Message);
                                break;
                            case TextReply textReply:
                                Bot.ReplyMessage(e.ReplyToken, textReply.Message);
                                break;
                        }

                        if (ret is Task task) await task;
                        await activator.OnAfterCommandAsync(service);
                    });
                }
            }
        }

        private static (string command, string args) SplitText(string text)
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
            
            _logger?.LogInformation("{TextMessage}", textMessage.ToString());
            var (command, args) = SplitText(textMessage.Text[1..]);

            switch (messageEvent.Source)
            {
                case GroupSource:
                    if (_groupCommandInvokers.TryGetValue(command, out var groupInvoker))
                        await groupInvoker(messageEvent, args);
                    break;
                case RoomSource:
                    if (_roomCommandInvokers.TryGetValue(command, out var roomInvoker))
                        await roomInvoker(messageEvent, args);
                    break;
                case UserSource:
                    if (_userCommandInvokers.TryGetValue(command, out var userInvoker))
                        await userInvoker(messageEvent, args);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e));
            }
        }

        public void UseLogging(IServiceProvider services)
        {
            _logger ??= services.GetRequiredService<ILogger<CommandManager>>();
        }
    }
}