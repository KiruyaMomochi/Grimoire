namespace Grimoire.Parser.Parameters
{
    public record SwitchParameter : Parameter
    {
        public uint? Lap { get; init; }
        public uint? Order { get; init; }
        public static readonly SwitchParameter Empty = new();
    }
}