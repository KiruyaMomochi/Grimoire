namespace Grimoire.Parser.Parameters
{
    public record ReserveListParameter : Parameter
    {
        public uint? Lap { get; init; }
        public uint? Order { get; init; }
        
        public static readonly ReserveListParameter Empty = new();
    }
}