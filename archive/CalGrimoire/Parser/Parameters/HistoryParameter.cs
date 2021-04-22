namespace Grimoire.Parser.Parameters
{
    public record HistoryParameter: Parameter
    {
        public uint? Lap { get; init; }
        public uint? Order { get; init; }
        public string Comment { get; init; }
        public static readonly ReserveParameter Empty = new();
    }
}