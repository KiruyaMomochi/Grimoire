using System;
using System.Threading.Tasks;
using Grimoire.LineApi.Event;
using Grimoire.LineApi.Message;

namespace Grimoire.Web.Commands
{
    public class CommandContext
    {
        public BaseEvent Event { get; init; }
        public string Args { get; set; }
    }

    public abstract class CommandBase
    {
        private CommandContext _commandContext;

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public CommandContext CommandContext
        {
            get => _commandContext;
            set => _commandContext = value ?? throw new ArgumentNullException(nameof(value));
        }

        public virtual Task OnInitializedAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnAfterCommandAsync()
        {
            return Task.CompletedTask;
        }

        public BaseEvent BaseEvent => CommandContext.Event;
        public MessageEvent MessageEvent => BaseEvent as MessageEvent;
        public TextMessage TextMessage => MessageEvent.Message as TextMessage;
        public string UserId => MessageEvent.Source.UserId;
        public string ReplyToken => MessageEvent.ReplyToken;
        public string Args => CommandContext.Args;
    }
}