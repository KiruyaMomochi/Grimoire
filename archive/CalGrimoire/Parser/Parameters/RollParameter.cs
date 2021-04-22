namespace Grimoire.Parser.Parameters
{
    public record RollParameter : Parameter
    {
        public int Delta { get; init; }

        public static readonly RollParameter Back = new RollParameter
        {
            Delta = -1
        };
        
        public static readonly RollParameter Forward = new RollParameter
        {
            Delta = 1
        };
    }
}