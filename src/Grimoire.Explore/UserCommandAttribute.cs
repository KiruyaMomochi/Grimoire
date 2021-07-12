using System;

namespace Grimoire.Explore
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class UserCommandAttribute : CommandAttribute
    {
        public UserCommandAttribute(string command) : base(SourceSet.User, command)
        {
        }

        public UserCommandAttribute(params string[] commands) : base(SourceSet.User, commands)
        {
        }
    }
}