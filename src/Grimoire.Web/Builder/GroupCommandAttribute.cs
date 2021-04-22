using System;
using System.Collections.Generic;

namespace Grimoire.Web.Builder
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class GroupCommandAttribute : System.Attribute
    {
        private readonly List<string> _commands = new();

        public GroupCommandAttribute(string command)
        {
            _commands.Add(command);
        }

        public GroupCommandAttribute(params string[] commands)
        {
            _commands.AddRange(commands);
        }

        public List<string> GetCommands() => _commands;
    }
}