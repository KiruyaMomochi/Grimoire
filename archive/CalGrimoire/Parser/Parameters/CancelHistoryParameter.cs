namespace Grimoire.Parser.Parameters
{
    public record CancelHistoryParameter : Parameter
    {
        public uint? Lap { get; init; }
        public uint? Order { get; init; }
        public static readonly CancelHistoryParameter Empty = new();
    }
}