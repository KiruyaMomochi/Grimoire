using System;
using Grimoire.Explore.Abstractions;
using Grimoire.Explore.Infrastructure;
using Grimoire.Line.Api.Webhook.Source;

#nullable enable
namespace Grimoire.Explore.Extensions
{
    public static class LineExtensions
    {
        public static bool HasSourceType(this CommandDescriptor commandDescriptor, SourceType sourceType)
        {
            switch (sourceType)
            {
                case SourceType.User:
                    if ((commandDescriptor.SourceSet & SourceSet.User) == 0)
                        return false;
                    break;
                case SourceType.Group:
                    if ((commandDescriptor.SourceSet & SourceSet.Group) == 0)
                        return false;
                    break;
                case SourceType.Room:
                    if ((commandDescriptor.SourceSet & SourceSet.Room) == 0)
                        return false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }
    }
}
#nullable restore
