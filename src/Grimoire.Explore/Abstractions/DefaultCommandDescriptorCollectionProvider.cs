using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Grimoire.Explore.Abstractions
{
    internal class DefaultCommandDescriptorCollectionProvider : ICommandDescriptorCollectionProvider
    {
        private readonly ICommandDescriptorProvider[] _commandDescriptorProviders;

        private readonly object _lock;
        private IReadOnlyList<CommandDescriptor> _collection;

        public IReadOnlyList<CommandDescriptor> CommandDescriptors
        {
            get
            {
                Initialize();
                Debug.Assert(_collection != null);
                return _collection;
            }
        }

        public DefaultCommandDescriptorCollectionProvider(IEnumerable<ICommandDescriptorProvider> commandDescriptorProviders)
        {
            _commandDescriptorProviders = commandDescriptorProviders.OrderBy(p => p.Order).ToArray();
            _lock = new object();
        }

        // public CommandDescriptorCollection

        private void Initialize()
        {
            if (_collection != null) return;
            lock (_lock)
                if (_collection == null)
                    UpdateCollection();
        }

        private void UpdateCollection()
        {
            lock (_lock)
            {
                var context = new CommandDescriptorProviderContext();

                foreach (var provider in _commandDescriptorProviders)
                    provider.OnProvidersExecuting(context);
                for (var i = _commandDescriptorProviders.Length - 1; i >= 0; i--)
                    _commandDescriptorProviders[i].OnProvidersExecuted(context);

                _collection = new List<CommandDescriptor>(context.Results);
            }
        }
    }
}