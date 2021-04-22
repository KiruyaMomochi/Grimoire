using System.Text.RegularExpressions;

namespace Grimoire.Parser
{
    internal static class CommandPattern
    {
        private const string Remain = @"(?<remain>\b(殘(刀)?|補(償)?)\b)";
        private const string Hang = @"(?<hang>\b掛(樹)?\b)";
        private const string Lap = @"(?<lap>\b\d+\b)";
        private const string Order = @"(?<order>\b\d+\b)";
        private const string Comment = @"(?<comment>[\w\W]+?)";

        private static readonly string Status = $@"(({Remain}|{Hang})\s*)*";
        private static readonly string Boss = $@"({Lap}\s*{Order})";
        private static readonly string OptionalBoss = $@"{Lap}?\s*{Order}";

        private static readonly string Report = $@"({Boss}?\s*{Status}{Comment}?\s*)";
        private static readonly string Reserve = $@"{OptionalBoss}?\s*{Remain}?\s*{Comment}?\s*";
        private static readonly string CancelReserve = $@"{OptionalBoss}?\s*{Comment}?\s*";
        private static readonly string Switch = $@"{OptionalBoss}.*";
        private static readonly string ReserveList = $@"{Lap}?\s*{Order}?";

        private static readonly RegexOptions RegexOptions =
            RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.ExplicitCapture;
        
        public static readonly Regex ReportPattern = new($@"^{Report}$", RegexOptions);
        public static readonly Regex ReservePattern = new($@"^{Reserve}$", RegexOptions);
        public static readonly Regex SwitchPattern = new($@"^{Switch}$", RegexOptions);
        public static readonly Regex ReserveListPattern = new($@"^{ReserveList}$", RegexOptions);
        public static readonly Regex CancelReservePattern = new($@"^{CancelReserve}$", RegexOptions);
    }
}