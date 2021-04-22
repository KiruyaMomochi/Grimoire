namespace Grimoire.Parser.Parameters
{
    public record CancelReserveParameter : Parameter
    {
        public uint? Lap { get; init; }
        public uint? Order { get; init; }
        public static readonly CancelReserveParameter Empty = new();
    }
}