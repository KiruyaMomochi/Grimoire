#nullable enable
using System;
using Grimoire.Explore.Abstractions;

namespace Grimoire.Explore.CommandRouting
{
    public class CommandMatchEntry : IEquatable<CommandMatchEntry>, IComparable<CommandMatchEntry>
    {
        // Reference: ConventionalRouteEntry

        public readonly CommandDescriptor CommandDescriptor;
        public readonly LineMessageDelegate LineMessageDelegate;

        public CommandMatchEntry(CommandDescriptor descriptor, LineMessageDelegate messageDelegate)
        {
            CommandDescriptor = descriptor;
            LineMessageDelegate = messageDelegate;
        }

        public int CompareTo(CommandMatchEntry? other)
        {
            if (other == null)
                return -1;
            
            var ls = CommandDescriptor.SourceSet;
            var rs = other.CommandDescriptor.SourceSet;
            if (ls != rs) return rs - ls; // TODO: which is better?

            var lts = CommandDescriptor.ParameterTypes;
            var rts = other.CommandDescriptor.ParameterTypes;
            var lc = lts.Count;
            var rc = rts.Count;
            if (lc != rc) return rc - lc;

            for (var i = 0; i < lc; i++)
            {
                var lt = (int) lts[i];
                var rt = (int) rts[i];

                if (lt != rt)
                    return lt - rt;
            }

            return 0;
        }

        public bool Equals(CommandMatchEntry? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return CommandDescriptor.Equals(other.CommandDescriptor) &&
                   LineMessageDelegate.Equals(other.LineMessageDelegate);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CommandMatchEntry) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CommandDescriptor, LineMessageDelegate);
        }

        public static bool operator ==(CommandMatchEntry? left, CommandMatchEntry? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CommandMatchEntry? left, CommandMatchEntry? right)
        {
            return !Equals(left, right);
        }
    }
}