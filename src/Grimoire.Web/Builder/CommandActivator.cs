using System;
using System.Reflection;
using Grimoire.Web.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Grimoire.Web.Builder
{
    public class CommandActivator
    {
        public Type Type;
        public MethodInfo Method;
        public Action<CommandBase, CommandContext> SetCommandContext;
        public ObjectFactory Activator;
    }
}