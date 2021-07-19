using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grimoire.Core.Services;
using Grimoire.Data;
using Grimoire.Data.Models;
using Grimoire.Explore;
using Grimoire.Explore.Attributes;
using Grimoire.Line.Api.Webhook.Source;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Grimoire.Core.Package
{
    /// <summary>
    /// Report System
    /// </summary>
    public class ReportPackage : PackageBase
    {
        private readonly ILogger<ReportPackage> _logger;
        private readonly GrimoireDatabaseContext _context;
        private readonly UsernameService _usernameService;
        private readonly StringBuilder _replyStringBuilder;

        public ReportPackage(ILogger<ReportPackage> logger, UsernameService usernameService, GrimoireDatabaseContext context)
        {
            _logger = logger;
            _context = context;
            _usernameService = usernameService;
            _replyStringBuilder = new StringBuilder();
        }

        private async Task<bool> IsGroupAllowed()
        {
            var source = (GroupSource) Event.Source;
            var g = await _context.Groups.FindAsync(source.GroupId);
            if (g != null)
                return true;

            _replyStringBuilder.AppendLine("The current group is not allowed, exiting.");
            _replyStringBuilder.AppendLine($"The group id is {source.GroupId}");

            var a = await _context.Admins.FindAsync(source.UserId);
            if (a != null)
                _replyStringBuilder.AppendLine("You can #add this group to allow list.");
            else
                _replyStringBuilder.AppendLine("Admins can #add this group to allow list.");
            
            return false;
        }

        public override async Task OnInitializedAsync()
        {
            await _usernameService.FetchUsernameAsync(Event.Source, _context);
        }

        private static readonly string CurrentNotSet = "未設定當前王，請使用 #切 設定後再試.";
        private static readonly string NoMatchRecords = "無符合條件的記錄.";
        private static readonly string NoHistory = "沒有可以回退的記錄.";

        /// <summary>
        /// Upsert sender to the current report list.
        /// </summary>
        /// <remarks>
        /// This command is available in the guild group.
        /// The report list of current boss will be replied.
        /// </remarks>
        /// <example>
        /// #報 新黑 ~ [Current report list]
        /// </example>
        [GroupCommand("1", "報", "報刀")]
        public async Task<string> Report()
        {
            if (!await IsGroupAllowed()) return BuildReply();

            var current = await _context.CurrentAsync();
            if (current == null)
                return CurrentNotSet;

            await _context.UpsertReport(current.Lap, current.Order, Args, Event.Source.UserId);
            await _context.SaveChangesAsync();
            AppendHistoryList(current.Lap, current.Order);

            return BuildReply();
        }

        /// <summary>
        /// Remove sender from the current report.
        /// </summary>
        /// <remarks>
        /// This command is available in the guild group.
        /// The report list of current boss will be replied.
        /// </remarks>
        /// <example>
        /// #取消報刀 ~ [Current report list]
        /// </example>
        [GroupCommand("取消", "取消報刀")]
        public async Task<string> CancelReport()
        {
            if (!await IsGroupAllowed()) return BuildReply();

            var current = await _context.CurrentAsync();
            if (current == null)
                return CurrentNotSet;

            var report = await _context.RemoveReport(current.Lap, current.Order, Event.Source.UserId);
            await _context.SaveChangesAsync();

            _replyStringBuilder.AppendLine(report == null ? "沒有記錄." : $"已移除記錄 {report.Comment}");
            AppendHistoryList(current.Lap, current.Order);

            return BuildReply();
        }

        /// <summary>
        /// Upsert sender to the specified report list.
        /// </summary>
        /// <remarks>
        /// This command is available in the guild group.
        /// The report list of current boss will be replied.
        /// </remarks>
        /// <example>
        /// #預約 1 2 新黑 ~ [Report list of 1 2]
        /// </example>
        [GroupCommand("2", "預約")]
        public async Task<string> Reserve(uint lap, uint order)
        {
            if (!await IsGroupAllowed()) return BuildReply();

            await _context.UpsertReport(lap, order, Args, Event.Source.UserId);
            await _context.SaveChangesAsync();
            AppendHistoryList(lap, order);

            return BuildReply();
        }

        /// <summary>
        /// Remove sender to the specified report list.
        /// If the boss is not specified, all sender's reserve not before
        /// current boss will be removed.
        /// </summary>
        /// <remarks>
        /// This command is available in the guild group.
        /// The report list of current boss will be replied if boss specified.
        /// Otherwise, the boss list where removing occurs will be replied.
        /// </remarks>
        /// <example>
        /// #取消預約 1 2 ~ [Report list of 1 2]
        /// #取消預約 ~ 已經移除 1-2, 2-3
        /// </example>
        /// TODO: split two functions
        [GroupCommand("取消預約")]
        public async Task<string> CancelReserve()
        {
            if (!await IsGroupAllowed()) return BuildReply();
            if (string.IsNullOrEmpty(Args)) return await CancelReportsGeqCurrent();

            if (!TryParseBoss(out var lap, out var order)) return BuildReply();

            await _context.RemoveReport(lap, order, Event.Source.UserId);
            await _context.SaveChangesAsync();
            AppendHistoryList(lap, order);
            return BuildReply();
        }

        private bool TryParseBoss(out uint lap, out uint order)
        {
            var args = ParseArgs(3);
            if (args.Length == 0)
            {
                _replyStringBuilder.Append("請指定週目和王.");
                lap = order = 0;
                return false;
            }

            if (args.Length == 1)
            {
                _replyStringBuilder.Append("除週目外，還需指定目標怪物.");
                lap = order = 0;
                return false;
            }

            GrimoireContext.Args = args.Length >= 3 ? args[2] : string.Empty;

            if (!uint.TryParse(args[0], out lap))
            {
                _replyStringBuilder.Append($"週目 {args[0]} 不能被轉換為數字.");
                order = 0;
                return false;
            }

            if (!uint.TryParse(args[1], out order))
            {
                _replyStringBuilder.Append($"怪物 {args[1]} 不能被轉換為數字.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Mark the current record of user to [save] state.
        /// If the comment is not set, use previous one.
        /// </summary>
        /// <remarks>
        /// This command is available in the guild group.
        /// The report list of current boss will be replied.
        /// </remarks>
        /// <example>
        /// #救 ~ [Current report list]
        /// #救 嗚嗚 ~ [Current report list]
        /// </example>
        [GroupCommand("救")]
        public async Task<string> Save()
        {
            if (!await IsGroupAllowed()) return BuildReply();

            var current = await _context.CurrentAsync();
            if (current == null) return CurrentNotSet;

            var report = await _context.Reports.FirstOrDefaultAsync(h =>
                h.Lap == current.Lap & h.Order == current.Order && h.UserId == Event.Source.UserId);

            if (report == null)
            {
                await _context.Reports.AddAsync(new Report
                {
                    Order = current.Order,
                    Lap = current.Lap,
                    Comment = Args,
                    IsFailed = true,
                    UserId = Event.Source.UserId
                });
            }
            else
            {
                report.IsFailed = true;

                var comment = Args;
                if (!string.IsNullOrEmpty(comment))
                    report.Comment = comment;
            }

            await _context.SaveChangesAsync();
            AppendHistoryList(current.Lap, current.Order);
            return BuildReply();
        }

        /// <summary>
        /// Switch to the next boss.
        /// </summary>
        /// <remarks>
        /// This command is available in the guild group.
        /// </remarks>
        /// <example>
        /// #倒 ~ Switched to Lap 1 Order 4
        /// </example>
        [GroupCommand("倒")]
        public async Task<string> SwitchNext()
        {
            if (!await IsGroupAllowed()) return BuildReply();
            var current = await _context.CurrentNoTrackingAsync();
            if (current == null) return CurrentNotSet;

            current.Advance(1);
            _context.Currents.Add(current with {Id = 0});
            await _context.SaveChangesAsync();
            return SwitchedTo(current.Lap, current.Order);
        }

        /// <summary>
        /// Switch to the last boss.
        /// </summary>
        /// <remarks>
        /// This command is available in the guild group.
        /// </remarks>
        /// <example>
        /// #回 ~ Switched to Lap 1 Order 4
        /// </example>
        [GroupCommand("回", "回退")]
        public async Task<string> UndoSwitch()
        {
            if (!await IsGroupAllowed()) return BuildReply();
            var last = await _context.Currents.OrderByDescending(c => c.Id).Take(2).ToListAsync();

            if (last.Count <= 1) return NoHistory;

            var last1 = last[0];
            var last2 = last[1];

            _context.Currents.Remove(last1);
            await _context.SaveChangesAsync();
            return SwitchedTo(last2.Lap, last2.Order);
        }

        /// <summary>
        /// Switch to the specific boss.
        /// </summary>
        /// <remarks>
        /// This command is available in the guild group.
        /// </remarks>
        /// <example>
        /// #切 1 4 ~ Switched to Lap 1 Order 4
        /// </example>
        [GroupCommand("切")]
        public async Task<string> Switch(uint lap, uint order)
        {
            if (!await IsGroupAllowed()) return BuildReply();

            var next = new Current {Lap = lap, Order = order};
            _context.Currents.Add(next);
            await _context.SaveChangesAsync();
            return SwitchedTo(next.Lap, next.Order);
        }
        
        // TODO: Temp impl!
        [GroupCommand("預約清單")]
        public async Task<string> ReportList()
        {
            if (!await IsGroupAllowed()) return BuildReply();

            var current = await _context.CurrentNoTrackingAsync();
            if (current == null) return CurrentNotSet;

            var group = _context.Reports
                .AsNoTracking()
                .Where(r => r.Lap > current.Lap || r.Lap == current.Lap && r.Order >= current.Order)
                .OrderBy(r => r.Lap)
                .ThenBy(r => r.Order)
                .ThenBy(r => r.Id)
                .Include(r => r.User)
                .ToList()
                .GroupBy(r => (r.Lap, r.Order), r => r);
            
            foreach (var g in group)
            {
                AppendReportListCore(g, $"{g.Key.Lap} 週 {g.Key.Order} 王", "");
            }

            return BuildReply();
        }

        private async Task<string> CancelReportsGeqCurrent()
        {
            var current = await _context.CurrentAsync();
            if (current == null)
            {
                return CurrentNotSet;
            }

            var reports = _context.Reports
                .Where(r =>
                    r.UserId == Event.Source.UserId &&
                    (r.Lap == current.Lap && r.Order >= current.Order || r.Lap > current.Lap));

            _context.Reports.RemoveRange(reports);
            await _context.SaveChangesAsync();

            var removed = string.Join(' ', reports.Select(x => $"{x.Lap}-{x.Order}"));
            return removed == "" ? NoMatchRecords : $"已移除: {removed}.";
        }


        private string[] ParseArgs(int count)
        {
            var res = Args?.Split((char[]) null, count,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return res ?? Array.Empty<string>();
        }

        private string[] ParseArgs()
        {
            var res = Args?.Split((char[]) null,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return res;
        }

        private void AppendHistoryList(uint lap, uint order, string prefix = "")
        {
            var reportsList = _context.ReportsList(lap, order);
            _replyStringBuilder.Append(prefix);
            AppendReportListCore(reportsList, $"報刀清單 - 目前是 {lap} 週 {order} 王\n", $"{lap} 週 {order} 王 無報刀記錄");
        }

        private void AppendReportListCore(IEnumerable<Report> reportsList, string hasReport, string noReport)
        {
            var idx = 0;
            foreach (var report in reportsList)
            {
                idx++;
                if (idx == 1)
                    _replyStringBuilder.AppendLine(hasReport);

                _replyStringBuilder.AppendFormat("{0,2:D}. ", idx);
                _replyStringBuilder.Append(report.User != null ? report.User.LineName : report.UserId[..6]);
                if (report.IsFailed)
                    _replyStringBuilder.Append(" [掛樹]");
                if (!string.IsNullOrWhiteSpace(report.Comment))
                    _replyStringBuilder.Append(" - ").Append(report.Comment);
                _replyStringBuilder.AppendLine();
            }

            if (idx == 0)
                _replyStringBuilder.AppendLine(noReport);
        }

        private string BuildReply() => _replyStringBuilder.ToString().Trim();

        private static string SwitchedTo(uint lap, uint order)
            => $"已切換到第 {lap} 週目第 {order} 王.";
    }
}
