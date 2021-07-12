using System;

namespace Grimoire.Explore
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class RoomCommandAttribute : CommandAttribute
    {
        public RoomCommandAttribute(string command) : base(SourceSet.Room, command)
        {
        }

        public RoomCommandAttribute(params string[] commands) : base(SourceSet.Room, commands)
        {
        }
    }
}