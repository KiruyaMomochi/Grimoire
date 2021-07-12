#nullable enable
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Grimoire.Explore.Infrastructure;
using Grimoire.Line.Api;
using Grimoire.Line.Api.Webhook.Event;
using Grimoire.Line.Api.Webhook.Message;
using Grimoire.Line.Api.Webhook.Source;

namespace Grimoire.Explore.CommandRouting
{
    public class CommandManager
    {
        private readonly ICommandDescriptorCollectionProvider _descriptorCollectionProvider;
        private readonly CommandMatchBuilder _commandMatchBuilder;
        private Dictionary<string, List<CommandMatchEntry>> _commandEndpointDict = new();
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
                return (null, null);
            var tokens = text.Split((char[]?) null, 2,
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

            // _logger?.LogInformation("{TextMessage}", textMessage.ToString());
            var (command, args) = SplitText(textMessage.Text[1..]);
            if (command == null) return;

            //var messageDelegate = _commandEndpointDict[new CommandRouteEntry(command, e.Source.SourceType)];
            // LineMessageDelegate messageDelegate =
            //     _commandEndpointDict[new CommandMatchEntry(command, e.Source.SourceType)];
            if (!_commandEndpointDict.TryGetValue(command, out var entries)) return;
            foreach (var entry in entries)
            {
                switch (e.Source.SourceType)
                {
                    case SourceType.User:
                        if ((entry.CommandDescriptor.SourceSet & SourceSet.User) == 0)
                            continue;
                        break;
                    case SourceType.Group:
                        if ((entry.CommandDescriptor.SourceSet & SourceSet.Group) == 0)
                            continue;
                        break;
                    case SourceType.Room:
                        if ((entry.CommandDescriptor.SourceSet & SourceSet.Room) == 0)
                            continue;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                var result = await entry.LineMessageDelegate(new GrimoireContext()
                {
                    Command = command,
                    Args = args,
                    Event = e
                });

                if (result == null) break;
                Console.WriteLine(JsonSerializer.Serialize(result, Options.SerializerOption));
                break;
            }
        }
    }
}