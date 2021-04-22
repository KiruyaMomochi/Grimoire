namespace Grimoire.Parser.Parameters
{
    public record ReportParameter : Parameter
    {
        public uint? Lap { get; init; }
        public uint? Order { get; init; }
        public string Comment { get; init; }
        public bool IsRemain { get; init; }
        public bool IsHang { get; init; }
        public static readonly ReportParameter Empty = new();
    }
}