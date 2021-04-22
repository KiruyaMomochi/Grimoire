namespace Grimoire.Parser.Parameters
{
    public record ReserveParameter : Parameter
    {
        public uint? Lap { get; init; }
        public uint? Order { get; init; }
        public string Comment { get; init; }
        public bool IsRemain { get; init; }
        public static readonly ReserveParameter Empty = new();
    }
}