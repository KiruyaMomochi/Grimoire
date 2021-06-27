namespace Grimoire.Explore.Abstractions
{
    public interface ICommandDescriptorProvider
    {
        void OnProvidersExecuting(CommandDescriptorProviderContext context);
        void OnProvidersExecuted(CommandDescriptorProviderContext context);
        int Order { get; }
    }
}