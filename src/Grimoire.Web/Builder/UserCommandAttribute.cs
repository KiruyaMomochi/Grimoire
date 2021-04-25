using System;
using System.Collections.Generic;

namespace Grimoire.Web.Builder
{
    [Flags]
    public enum SourceSet
    {
        None,
        User,
        Group,
        Room
    }

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
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class GroupCommandAttribute : CommandAttribute
    {
        public GroupCommandAttribute(string command): base(SourceSet.Group, command)
        {
        }

        public GroupCommandAttribute(params string[] commands): base(SourceSet.Group, commands)
        {
        }
    }
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class UserCommandAttribute : CommandAttribute
    {
        public UserCommandAttribute(string command): base(SourceSet.User, command)
        {
        }

        public UserCommandAttribute(params string[] commands): base(SourceSet.User, commands)
        {
        }
    }
}