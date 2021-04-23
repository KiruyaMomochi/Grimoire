using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grimoire.Web.Builder;
using Grimoire.Web.Models;
using Grimoire.Web.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Grimoire.Web.Commands
{
    /// <summary>
    /// Report System
    /// </summary>
    /// TODO: Use a global string builder to represent reply mesage
    /// TODO: Reply message by return a string
    public class ReportSystem : CommandBase
    {
        private readonly ILogger<ReportSystem> _logger;
        private readonly GrimoireContext _context;
        private readonly IBotService _botService;
        private readonly UsernameService _usernameService;

        public ReportSystem(ILogger<ReportSystem> logger, IBotService botService, UsernameService usernameService,
            GrimoireContext context)
        {
            _logger = logger;
            _context = context;
            _botService = botService;
            _usernameService = usernameService;
        }

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
        public async Task Report()
        {
            var current = await _context.CurrentAsync();
            if (current == null)
            {
                ReplyCurrentNotSet();
                return;
            }

            await _usernameService.FetchUsernameAsync(MessageEvent.Source, _context);
            await _context.UpsertReport(current.Lap, current.Order, Args, UserId);
            await _context.SaveChangesAsync();
            ReplyHistoryList(current.Lap, current.Order);
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
        [GroupCommand("取消")]
        public async Task CancelReport()
        {
            var current = await _context.CurrentAsync();
            if (current == null)
            {
                ReplyCurrentNotSet();
                return;
            }

            await _context.RemoveReport(current.Lap, current.Order, UserId);
            await _context.SaveChangesAsync();
            ReplyHistoryList(current.Lap, current.Order);
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
        public async Task Reserve()
        {
            if (!TryParseBoss(out var lap, out var order))
                return;

            await _usernameService.FetchUsernameAsync(MessageEvent.Source, _context);
            await _context.UpsertReport(lap, order, Args, UserId);
            await _context.SaveChangesAsync();
            ReplyHistoryList(lap, order);
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
        public async Task CancelReserve()
        {
            if (string.IsNullOrEmpty(Args))
            {
                await CancelReportsGeqCurrent();
                return;
            }

            if (!TryParseBoss(out var lap, out var order))
                return;

            await _context.RemoveReport(lap, order, UserId);
            await _context.SaveChangesAsync();
            ReplyHistoryList(lap, order);
        }

        private bool TryParseBoss(out uint lap, out uint order)
        {
            var args = ParseArgs(3);
            if (args.Length == 0)
            {
                ReplyMessage("請指定週目和王.");
                lap = order = 0;
                return false;
            }
            if (args.Length == 1)
            {
                ReplyMessage("除週目外，還需指定目標怪物.");
                lap = order = 0;
                return false;
            }
            
            CommandContext.Args = args.Length >= 3 ? args[2] : string.Empty;
            
            if (!uint.TryParse(args[0], out lap))
            {
                ReplyMessage($"週目 {args[0]} 不能被轉換為數字.");
                order = 0;
                return false;
            }

            if (!uint.TryParse(args[1], out order))
            {
                ReplyMessage($"怪物 {args[1]} 不能被轉換為數字.");
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
        public async Task Save()
        {
            var current = await _context.CurrentAsync();
            if (current == null)
            {
                ReplyCurrentNotSet();
                return;
            }

            var report = await _context.Reports.FirstOrDefaultAsync(h =>
                h.Lap == current.Lap & h.Order == current.Order && h.UserId == UserId);

            if (report == null)
            {
                report = new Report
                {
                    Order = current.Order,
                    Lap = current.Lap,
                    Comment = Args,
                    IsFailed = true,
                    UserId = UserId
                };
                await _context.Reports.AddAsync(report);
            }
            else
            {
                report.IsFailed = true;

                var comment = Args;
                if (!string.IsNullOrEmpty(comment))
                    report.Comment = comment;
            }

            await _context.SaveChangesAsync();
            ReplyHistoryList(current.Lap, current.Order);
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
        public async Task SwitchNext()
        {
            var current = await _context.CurrenNoTrackingtAsync();
            if (current == null)
            {
                ReplyCurrentNotSet();
                return;
            }

            current.Advance(1);
            _context.Currents.Add(current with {Id = 0});
            await _context.SaveChangesAsync();
            ReplySwitchedTo(current.Lap, current.Order);
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
        public async Task UndoSwitch()
        {
            var last = await _context.Currents.OrderByDescending(c => c.Id).Take(2).ToListAsync();

            if (last.Count <= 1)
            {
                ReplyMessage("沒有可以回退的記錄.");
                return;
            }

            var last1 = last[0];
            var last2 = last[1];

            _context.Currents.Remove(last1);
            await _context.SaveChangesAsync();
            ReplySwitchedTo(last2.Lap, last2.Order);
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
        public async Task Switch()
        {
            if (!TryParseBoss(out var lap, out var order))
                return;

            var next = new Current() {Lap = lap, Order = order};
            _context.Currents.Add(next);
            await _context.SaveChangesAsync();
            ReplySwitchedTo(next.Lap, next.Order);
        }

        private async Task CancelReportsGeqCurrent()
        {
            var current = await _context.CurrentAsync();
            if (current == null)
            {
                ReplyCurrentNotSet();
                return;
            }

            var reports = _context.Reports
                .Where(r =>
                    r.UserId == UserId &&
                    (r.Lap == current.Lap && r.Order >= current.Order || r.Lap > current.Lap));

            _context.Reports.RemoveRange(reports);
            await _context.SaveChangesAsync();

            var removed = string.Join(' ', reports.Select(x => $"{x.Lap}-{x.Order}"));
            ReplyMessage(removed == "" ? "無符合條件的記錄." : $"已移除: {removed}.");
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

        private void ReplyHistoryList(uint lap, uint order, string prefix = "")
        {
            var reportsList = _context.ReportsList(lap, order);
            var sb = new StringBuilder();

            sb.Append(prefix);

            var idx = 0;
            foreach (var report in reportsList)
            {
                idx++;
                sb.AppendFormat("{0,2:D}. ", idx);
                sb.Append(report.User != null ? report.User.LineName : report.UserId[..6]);
                if (report.IsFailed)
                    sb.Append(" [掛樹]");
                if (report.Comment != null)
                    sb.Append(" - ").Append(report.Comment);
                sb.AppendLine();
            }

            if (idx == 0)
                sb.Append(lap).Append('週').Append(order).Append('王').Append(" 無報刀記錄");
            else
                sb.Insert(0, $"報刀清單 - 目前是{lap}週{order}王\n");

            ReplyMessage(sb.ToString().Trim());
        }

        private void ReplyMessage(string message) => _botService.ReplyMessage(ReplyToken, message);
        
        
        private void ReplyCurrentNotSet()
        {
            ReplyMessage("未設定當前王，請使用 #切 設定後再試.");;
        }

        private void ReplySwitchedTo(uint lap, uint order)
        {
            ReplyMessage($"已切換到第 {lap} 週目第 {order} 王.");
        }
    }
}