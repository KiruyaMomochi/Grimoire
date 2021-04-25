using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grimoire.LineApi.Source;
using Grimoire.Web.Builder;
using Grimoire.Web.Models;
using Grimoire.Web.Replies;
using Grimoire.Web.Services;
using isRock.LineBot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Grimoire.Web.Commands
{
    /// <summary>
    /// Report System
    /// </summary>
    /// TODO: Use a global string builder to represent reply message
    /// TODO: add a method that will execute before all the commands
    public class ReportSystem : CommandBase
    {
        private readonly ILogger<ReportSystem> _logger;
        private readonly GrimoireContext _context;
        private readonly UsernameService _usernameService;
        private readonly StringBuilder _replyStringBuilder;

        public ReportSystem(ILogger<ReportSystem> logger, UsernameService usernameService, GrimoireContext context)
        {
            _logger = logger;
            _context = context;
            _usernameService = usernameService;
            _replyStringBuilder = new StringBuilder();
        }

        private async Task<bool> IsGroupAllowed()
        {
            var source = (GroupSource) MessageEvent.Source;
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
            await _usernameService.FetchUsernameAsync(MessageEvent.Source, _context);
        }

        private static readonly TextReply CurrentNotSet = new("未設定當前王，請使用 #切 設定後再試.");
        private static readonly TextReply NoMatchRecords = new("無符合條件的記錄.");
        private static readonly TextReply NoHistory = new("沒有可以回退的記錄.");

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
        public async Task<TextReply> Report()
        {
            if (!await IsGroupAllowed()) return BuildReply();

            var current = await _context.CurrentAsync();
            if (current == null)
                return CurrentNotSet;

            await _context.UpsertReport(current.Lap, current.Order, Args, UserId);
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
        public async Task<TextReply> CancelReport()
        {
            if (!await IsGroupAllowed()) return BuildReply();

            var current = await _context.CurrentAsync();
            if (current == null)
                return CurrentNotSet;

            var report = await _context.RemoveReport(current.Lap, current.Order, UserId);
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
        public async Task<TextReply> Reserve()
        {
            if (!await IsGroupAllowed()) return BuildReply();
            if (!TryParseBoss(out var lap, out var order)) return BuildReply();

            await _context.UpsertReport(lap, order, Args, UserId);
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
        [GroupCommand("取消預約")]
        public async Task<TextReply> CancelReserve()
        {
            if (!await IsGroupAllowed()) return BuildReply();
            // This is slow
            if (string.IsNullOrEmpty(Args)) return await CancelReportsGeqCurrent();

            if (!TryParseBoss(out var lap, out var order)) return BuildReply();

            await _context.RemoveReport(lap, order, UserId);
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

            CommandContext.Args = args.Length >= 3 ? args[2] : string.Empty;

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
        public async Task<TextReply> Save()
        {
            if (!await IsGroupAllowed()) return BuildReply();

            var current = await _context.CurrentAsync();
            if (current == null) return CurrentNotSet;

            var report = await _context.Reports.FirstOrDefaultAsync(h =>
                h.Lap == current.Lap & h.Order == current.Order && h.UserId == UserId);

            if (report == null)
            {
                await _context.Reports.AddAsync(new Report
                {
                    Order = current.Order,
                    Lap = current.Lap,
                    Comment = Args,
                    IsFailed = true,
                    UserId = UserId
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
        public async Task<TextReply> SwitchNext()
        {
            if (!await IsGroupAllowed()) return BuildReply();
            var current = await _context.CurrenNoTrackingtAsync();
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
        public async Task<TextReply> UndoSwitch()
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
        public async Task<TextReply> Switch()
        {
            if (!await IsGroupAllowed()) return BuildReply();
            if (!TryParseBoss(out var lap, out var order)) return BuildReply();

            var next = new Current {Lap = lap, Order = order};
            _context.Currents.Add(next);
            await _context.SaveChangesAsync();
            return SwitchedTo(next.Lap, next.Order);
        }

        private async Task<TextReply> CancelReportsGeqCurrent()
        {
            var current = await _context.CurrentAsync();
            if (current == null)
            {
                return CurrentNotSet;
            }

            var reports = _context.Reports
                .Where(r =>
                    r.UserId == UserId &&
                    (r.Lap == current.Lap && r.Order >= current.Order || r.Lap > current.Lap));

            _context.Reports.RemoveRange(reports);
            await _context.SaveChangesAsync();

            var removed = string.Join(' ', reports.Select(x => $"{x.Lap}-{x.Order}"));
            return removed == "" ? NoMatchRecords : new TextReply($"已移除: {removed}.");
        }


        private string[] ParseArgs(int count)
        {
            var res = CommandContext.Args?.Split((char[]) null, count,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return res ?? Array.Empty<string>();
        }

        private string[] ParseArgs()
        {
            var res = CommandContext.Args?.Split((char[]) null,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return res;
        }

        private void AppendHistoryList(uint lap, uint order, string prefix = "")
        {
            var reportsList = _context.ReportsList(lap, order);

            _replyStringBuilder.Append(prefix);

            var idx = 0;
            foreach (var report in reportsList)
            {
                idx++;
                if (idx == 1)
                    _replyStringBuilder.Insert(0, $"報刀清單 - 目前是{lap}週{order}王\n");

                _replyStringBuilder.AppendFormat("{0,2:D}. ", idx);
                _replyStringBuilder.Append(report.User != null ? report.User.LineName : report.UserId[..6]);
                if (report.IsFailed)
                    _replyStringBuilder.Append(" [掛樹]");
                if (!string.IsNullOrWhiteSpace(report.Comment))
                    _replyStringBuilder.Append(" - ").Append(report.Comment);
                _replyStringBuilder.AppendLine();
            }

            if (idx == 0)
                _replyStringBuilder.Append(lap).Append('週').Append(order).Append('王').Append(" 無報刀記錄");
        }

        private TextReply BuildReply() => new(_replyStringBuilder.ToString().Trim());

        private static TextReply SwitchedTo(uint lap, uint order)
            => new($"已切換到第 {lap} 週目第 {order} 王.");
    }
}