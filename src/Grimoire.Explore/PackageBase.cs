using System;
using Grimoire.Line.Api.Webhook.Event;

#nullable enable

namespace Grimoire.Explore
{
    [Package]
    public abstract class PackageBase
    {
        private GrimoireContext? _grimoireContext;
        public BaseEvent Event => GrimoireContext.Event;

        public GrimoireContext GrimoireContext
        {
            get
            {
                _grimoireContext ??= new GrimoireContext();
                return _grimoireContext;
            }
            set => _grimoireContext = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
