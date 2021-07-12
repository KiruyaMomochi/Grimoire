using System;

namespace Grimoire.Explore
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class GroupCommandAttribute : CommandAttribute
    {
        public GroupCommandAttribute(string command) : base(SourceSet.Group, command)
        {
        }

        public GroupCommandAttribute(params string[] commands) : base(SourceSet.Group, commands)
        {
        }
    }
}