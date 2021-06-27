#nullable enable
using System;
using Grimoire.Explore.Abstractions;

namespace Grimoire.Explore.CommandRouting
{
    public class CommandMatchEntry : IEquatable<CommandMatchEntry>
    {
        // Reference: ConventionalRouteEntry

        public readonly CommandDescriptor CommandDescriptor;
        public readonly LineMessageDelegate LineMessageDelegate;

        public CommandMatchEntry(CommandDescriptor descriptor, LineMessageDelegate messageDelegate)
        {
            CommandDescriptor = descriptor;
            LineMessageDelegate = messageDelegate;
        }

        public static bool operator <(CommandMatchEntry? left, CommandMatchEntry? right)
        {
            if (right == null)
                return false;
            if (left == null)
                return true;

            var ls = left.CommandDescriptor.SourceSet;
            var rs = right.CommandDescriptor.SourceSet;
            if (ls != rs) return ls < rs;

            var lp = left.CommandDescriptor.Parameters;
            var rp = right.CommandDescriptor.Parameters;
            var lc = lp.Count;
            var rc = rp.Count;

            if (lc != rc) return lc < rc;
            for (int i = 0; i < lc; i++)
            {
                var lt = lp[i].ParameterType;
                var rt = rp[i].ParameterType;
                if (lt != typeof(int) && rt == typeof(int))
                    return false;
                if (lt == typeof(int) && rt != typeof(int))
                    return true;
            }

            return false;
        }

        public static bool operator >(CommandMatchEntry? left, CommandMatchEntry? right)
        {
            if (left == null)
                return false;
            if (right == null)
                return true;

            var ls = left.CommandDescriptor.SourceSet;
            var rs = right.CommandDescriptor.SourceSet;
            if (ls != rs) return ls > rs;

            var lp = left.CommandDescriptor.Parameters;
            var rp = right.CommandDescriptor.Parameters;
            var lc = lp.Count;
            var rc = rp.Count;

            if (lc != rc) return lc > rc;
            for (int i = 0; i < lc; i++)
            {
                var lt = lp[i].ParameterType;
                var rt = rp[i].ParameterType;
                if (lt == typeof(int) && rt != typeof(int))
                    return false;
                if (lt != typeof(int) && rt == typeof(int))
                    return true;
            }

            return false;
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