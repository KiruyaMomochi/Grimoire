namespace Grimoire.Parser.Parameters
{
    public record NoneParameter : Parameter
    {
        public static readonly NoneParameter Empty = new();
    }
}