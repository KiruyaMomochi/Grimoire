namespace Grimoire.Parser.Parameters
{
    public record ErrorParameter : Parameter
    {
        public string Message { get; init; }
        public static ErrorParameter Empty = new ErrorParameter();
    }
}