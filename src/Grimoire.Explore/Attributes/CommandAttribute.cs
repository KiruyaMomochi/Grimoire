using System;
using System.Collections.Generic;
using Grimoire.Explore.Infrastructure;

namespace Grimoire.Explore.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class CommandAttribute : Attribute
    {
        public SourceSet SourceSet { get; protected set; }
        public List<string> Commands { get; protected set; } = new List<string>();
        
        public CommandAttribute(SourceSet sourceSet, string command)
        {
            SourceSet = sourceSet;
            Commands.Add(command);
        }

        public CommandAttribute(SourceSet sourceSet, params string[] commands)
        {
            SourceSet = sourceSet;
            Commands.AddRange(commands);
        }

        public CommandAttribute(params string[] commands)
        {
            SourceSet = SourceSet.Group | SourceSet.Room | SourceSet.User;
            Commands.AddRange(commands);
        }
    }
}
