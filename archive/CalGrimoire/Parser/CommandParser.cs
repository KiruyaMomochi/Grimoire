using System;
using Grimoire.Parser.Parameters;
// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.DelegateAllocation

namespace Grimoire.Parser
{
    public static class CommandParser
    {
        public static void AddReportSystem(this Parser parser)
        {
            parser
                .AddGroupParser(ParseReport, "1", "報", "報刀")
                .AddGroupParser(ParseCancelReport, "取消", "取消報刀")
                .AddGroupParser(ParseReserve, "2", "約", "預約")
                .AddGroupParser(ParseHang, "救", "掛樹")
                .AddGroupParser(ParseRoll, "倒")
                .AddGroupParser(ParseRollBack, "退", "回退")
                .AddGroupParser(ParseSwitch, "切", "切換")
                .AddGroupParser(ParseReportList, "報", "報刀清單");

            parser
                .AddUserParser(ParseReserveList, "P", "預約清單")
                .AddUserParser(ParseClearReports, "清空刀表");
        }

        public static void AddFeedbackSystem(this Parser parser)
        {
            parser
                .AddGroupParser(ParseHistory, "F", "回報")
                .AddGroupParser(ParseUndoHistory, "清", "清除")
                .AddGroupParser(ParseRemainList, "S", "剩餘清單")
                .AddGroupParser(ParseRemainNumberList, "C", "剩餘刀數")
                .AddGroupParser(ParseRemainNumberList, "U", "未出清單");

            parser
                .AddUserParser(ParseHistoryRecord, "F", "記錄");
        }

        public static void AddMemberSystem(this Parser parser)
        {
            parser
                .AddGroupParser(ParseMemberList, "成員清單")
                .AddGroupParser(ParseNickname, "昵稱");
            
            parser
                .AddUserParser(ParseMemberList, "成員清單")
                .AddUserParser(ParseRemoveUser, "刪", "刪除");
        }

        private static Parameter ParseNickname(string s)
        {
            throw new NotImplementedException();
        }

        private static Parameter ParseMemberList(string s)
        {
            throw new NotImplementedException();
        }

        private static Parameter ParseNoAttemptList(string s)
        {
            throw new NotImplementedException();
        }

        private static Parameter ParseRemainNumberList(string s)
        {
            throw new NotImplementedException();
        }

        private static Parameter ParseRemainList(string s) 
        {
            throw new NotImplementedException();
        }

        private static Parameter ParseUndoHistory(string s)
        {
            throw new NotImplementedException();
        }

        private static Parameter ParseCancelReport(string arg)
        {
            throw new NotImplementedException();
        }

        private static Parameter ParseRemoveUser(string s)
        {
            throw new NotImplementedException();
        }

        private static Parameter ParseHistoryRecord(string s)
        { 
            throw new NotImplementedException();
        }

        private static Parameter ParseClearReports(string s)
        {
            throw new NotImplementedException();
        }

        private static Parameter ParseReportList(string s)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 基礎報刀指令
        /// #報刀 #報 #1 [備註]
        /// 使用範圍    [公會成員和管理員]公會群組中
        /// 回覆訊息    輸出報刀清單
        /// 備註       只能報當前王
        /// </summary>
        /// <param name="arg">傳入的指令</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static HistoryParameter ParseHistory(string arg) 
            => new() {Comment = arg};

        /// <summary>
        /// Parse the reserve list command.
        /// This command has no arguments.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns>An <c>ReserveListParameter</c> object.</returns>
        /// <exception cref="NotImplementedException"></exception>
        private static ReserveListParameter ParseReserveList(string arg)
        {
            var tokens = arg.Split((char[])null, 3,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var match = CommandPattern.ReserveListPattern.Match(arg);
            return new ReserveListParameter
            {
                Lap = uint.TryParse(match.Groups["lap"].Value, out var lap) ? lap : null,
                Order = uint.TryParse(match.Groups["order"].Value, out var order) ? order : null
            };
        }

        /// <summary>
        /// Parse the switch command.
        /// The command syntax is
        /// <code>
        /// [lap] order
        /// </code>
        /// </summary>
        /// <param name="arg">The argument of this command.</param>
        /// <returns>An <c>RollParameter</c> object that contains options about this command.</returns>
        private static SwitchParameter ParseSwitch(string arg)
        {
            var tokens = arg.Split((char[])null, 3,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var match = CommandPattern.SwitchPattern.Match(arg);
            return new SwitchParameter
            {
                Lap = uint.TryParse(match.Groups["lap"].Value, out var lap) ? lap : null,
                Order = uint.TryParse(match.Groups["order"].Value, out var order) ? order : null
            };
        }

        /// <summary>
        /// Parse the roll command.
        /// The command syntax is
        /// <code>
        /// [integer]
        /// </code>
        /// </summary>
        /// <param name="arg">The argument of this command.</param>
        /// <returns>An <c>RollParameter</c> object that contains options about this command.</returns>
        private static RollParameter ParseRoll(string arg)
        {
            if (arg == "")
                return RollParameter.Forward;

            return new RollParameter
            {
                Delta = int.Parse(arg)
            };
        }

        /// <summary>
        /// Parse the roll back command.
        /// The command syntax is
        /// <code>
        /// [integer]
        /// </code>
        /// </summary>
        /// <param name="arg">The argument of this command.</param>
        /// <returns>An <c>RollParameter</c> object that contains options about this command.</returns>
        private static RollParameter ParseRollBack(string arg)
        {
            if (arg == "")
                return RollParameter.Back;

            return int.TryParse(arg, out var delta)
                ? new RollParameter { Delta = -delta }
                : RollParameter.Back;
        }

        /// <summary>
        /// Parse the cancel reserve command.
        /// The command syntax is
        /// <code>
        /// [lap] [order]
        /// </code>
        /// </summary>
        /// <param name="arg">The argument of this command.</param>
        /// <returns>An <c>CancelReserveParameter</c> object that contains options about this command.</returns>
        private static CancelReserveParameter ParseCancelReserve(string arg)
        {
            var match = CommandPattern.CancelReservePattern.Match(arg);

            return new CancelReserveParameter
            {
                Lap = uint.TryParse(match.Groups["lap"].Value, out var lap) ? lap : null,
                Order = uint.TryParse(match.Groups["order"].Value, out var order) ? order : null,
            };
        }
        
        /// <summary>
        /// Parse the cancel history command.
        /// The command syntax is
        /// <code>
        /// [lap] [order]
        /// </code>
        /// </summary>
        /// <param name="arg">The argument of this command.</param>
        /// <returns>An <c>CancelReserveParameter</c> object that contains options about this command.</returns>
        private static CancelHistoryParameter ParseCancelHistory(string arg)
        {
            var match = CommandPattern.CancelReservePattern.Match(arg);

            return new CancelHistoryParameter
            {
                Lap = uint.TryParse(match.Groups["lap"].Value, out var lap) ? lap : null,
                Order = uint.TryParse(match.Groups["order"].Value, out var order) ? order : null,
            };
        }

        /// <summary>
        /// Parse the reserve command.
        /// The command syntax is
        /// <code>
        /// [digit] digit [remain] [comment]
        /// </code>
        /// </summary>
        /// <param name="arg">The argument of this command.</param>
        /// <returns>An <c>ReserveParameter</c> object that contains options about this command.</returns>
        private static ReserveParameter ParseReserve(string arg)
        {
            var match = CommandPattern.ReservePattern.Match(arg);
            return new ReserveParameter
            {
                Lap = uint.TryParse(match.Groups["lap"].Value, out var lap) ? lap : null,
                Order = uint.TryParse(match.Groups["order"].Value, out var order) ? order : null,
                IsRemain = match.Groups["remain"].Success,
                Comment = match.Groups["comment"].Success ? match.Groups["comment"].Value : null,
            };
        }

        /// <summary>
        /// Parse the hang command.
        /// The command syntax is
        /// <code>
        /// [digit digit] [status] [comment]
        /// </code>
        /// </summary>
        /// <param name="arg">The argument of this command.</param>
        /// <returns>An <c>ReportParameter</c> object that contains options about this command.</returns>
        private static ReportParameter ParseHang(string arg)
        {
            return ParseReport(arg) with { IsHang = true };
        }

        /// <summary>
        /// Parse the remain command.
        /// The command syntax is
        /// <code>
        /// [digit digit] [status] [comment]
        /// </code>
        /// </summary>
        /// <param name="arg">The argument of this command.</param>
        /// <returns>An <c>ReportParameter</c> object that contains options about this command.</returns>
        private static ReportParameter ParseRemain(string arg)
        {
            return ParseReport(arg) with { IsRemain = true };
        }

        /// <summary>
        /// Parse the report command.
        /// The command syntax is
        /// <code>
        /// [digit digit] [status] [comment]
        /// </code>
        /// </summary>
        /// <param name="arg">The argument of this command.</param>
        /// <returns>An <c>ReportParameter</c> object that contains options about this command.</returns>
        private static ReportParameter ParseReport(string arg)
        {
            var match = CommandPattern.ReportPattern.Match(arg);
            return new ReportParameter
            {
                Lap = uint.TryParse(match.Groups["lap"].Value, out var lap) ? lap : null,
                Order = uint.TryParse(match.Groups["order"].Value, out var order) ? order : null,
                IsRemain = match.Groups["remain"].Success,
                IsHang = match.Groups["hang"].Success,
                Comment = match.Groups["comment"].Success ? match.Groups["comment"].Value : null,
            };
        }
    }
}