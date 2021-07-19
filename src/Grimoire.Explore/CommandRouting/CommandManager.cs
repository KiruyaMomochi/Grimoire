#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Grimoire.Explore.Abstractions;
using Grimoire.Explore.Extensions;
using Grimoire.Explore.Infrastructure;
using Grimoire.Explore.Parameter;
using Grimoire.Line.Api;
using Grimoire.Line.Api.Webhook.Event;
using Grimoire.Line.Api.Webhook.Message;
using Grimoire.Line.Api.Webhook.Source;
using BaseMessage = Grimoire.Line.Api.Message.BaseMessage;

namespace Grimoire.Explore.CommandRouting
{
    public class CommandMatchEntrySet
    {
        public List<CommandMatchEntry> CommandMatchEntries { get; set; } = new List<CommandMatchEntry>();
        public int MaxParametersCount { get; set; }

        public int Count => CommandMatchEntries.Count;

        public CommandMatchEntry this[int i]
        {
            get => CommandMatchEntries[i];
            set => CommandMatchEntries[i] = value;
        }
        
        public void Add(CommandMatchEntry commandMatchEntry)
        {
            CommandMatchEntries.Add(commandMatchEntry);

            var count = commandMatchEntry.CommandDescriptor.Parameters.Count;
            if (count > MaxParametersCount) MaxParametersCount = count;
        }
    }

    public class CommandManager
    {
        private readonly ICommandDescriptorCollectionProvider _descriptorCollectionProvider;
        private readonly CommandMatchBuilder _commandMatchBuilder;
        private Dictionary<string, CommandMatchEntrySet> _commandEndpointDict = new();
        private readonly char _startSymbol = '#';

        public CommandManager(ICommandDescriptorCollectionProvider descriptorCollectionProvider,
            CommandMatchBuilder commandMatchBuilder)
        {
            _descriptorCollectionProvider = descriptorCollectionProvider;
            _commandMatchBuilder = commandMatchBuilder;
        }

        public void Collect()
        {
            foreach (var descriptor in _descriptorCollectionProvider.CommandDescriptors)
            {
                _commandMatchBuilder.Add(descriptor);
            }

            _commandEndpointDict = _commandMatchBuilder.Build();
        }

        private static (string? command, string? args) SplitText(string? text)
        {
            if (text == null)
                return (null, string.Empty);
            var tokens = text.Split(new[] {' ', '　', '\n'}, 2,
                StringSplitOptions.RemoveEmptyEntries);
            return tokens.Length switch
            {
                0 => (null, null),
                1 => (tokens[0].Trim(), null),
                2 => (tokens[0].Trim(), tokens[1]),
                _ => throw new IndexOutOfRangeException()
            };
        }

        public async Task HandleWebhookEvent(BaseEvent e)
        {
            if (e is not MessageEvent {Message: TextMessage textMessage} messageEvent) return;
            if (!textMessage.Text.StartsWith(_startSymbol))
                return;

            var (command, args) = SplitText(textMessage.Text[1..]);
            if (command == null) return;

            if (!_commandEndpointDict.TryGetValue(command, out var entries)) return;
            BaseMessage? result = null;

            var messageParametersDescriptor = new MessageParametersDescriptor(args, entries.MaxParametersCount);

            foreach (var entry in entries.CommandMatchEntries)
            {
                if (!entry.CommandDescriptor.HasSourceType(e.Source.SourceType))
                    continue;

                var parameterTypes = entry.CommandDescriptor.ParameterTypes!;

                object[] parameters = Array.Empty<object>();
                if (parameterTypes.Count != 0)
                {
                    parameters = new object[parameterTypes.Count];
                    if (!messageParametersDescriptor.TryBuildParameters(parameterTypes, parameters))
                        continue;
                }

                result = await entry.LineMessageDelegate(new GrimoireContext
                {
                    Command = command,
                    RealArgs = parameters,
                    Args = messageParametersDescriptor.BuildRemainParameters(args, parameters.Length),
                    Event = e
                });

                break;
            }

            if (result == null) return;
            // TODO: Send result
            Console.WriteLine(JsonSerializer.Serialize(result, Options.SerializerOption));
        }
    }
}
