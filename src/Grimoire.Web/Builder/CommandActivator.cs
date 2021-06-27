using System;
using System.Reflection;
using System.Threading.Tasks;
using Grimoire.Web.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Grimoire.Web.Builder
{
    public class CommandActivator
    {
        public Type Type;
        public MethodInfo Method;

        public delegate Task AsyncLifecycleDelegate(SystemBase service);
        
        public AsyncLifecycleDelegate OnInitializedAsync;
        public AsyncLifecycleDelegate OnAfterCommandAsync;
        
        public Action<SystemBase, CommandContext> SetCommandContext;
        public ObjectFactory Factory;
    }
}