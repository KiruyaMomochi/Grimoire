using System;
using System.Threading.Tasks;
using Grimoire.Explore.Attributes;
using Grimoire.Explore.Infrastructure;
using Grimoire.Line.Api.Webhook.Event;

#nullable enable

namespace Grimoire.Explore
{
    [Package]
    public abstract class PackageBase
    {
        private GrimoireContext? _grimoireContext;
        public BaseEvent Event => GrimoireContext.Event;
        public string Args => GrimoireContext.Args;
        

        public GrimoireContext GrimoireContext
        {
            get
            {
                _grimoireContext ??= new GrimoireContext();
                return _grimoireContext;
            }
            set => _grimoireContext = value ?? throw new ArgumentNullException(nameof(value));
        }

        public virtual Task OnInitializedAsync() => Task.CompletedTask;
    }
}
