using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;

namespace Grimoire.Web.Models
{
    public static class GrimoireContextExtensions
    {
        public static IQueryable<Report> ReportsAt(this GrimoireContext context, uint lap, uint order) =>
            context.Reports.Where(reserve => reserve.Lap == lap && reserve.Order == order)
                .OrderBy(h => h.Id);

        public static IQueryable<Report> ReportsAfter(this GrimoireContext context, uint lap, uint order) =>
            context.Reports.Where(reserve => reserve.Lap == lap && reserve.Order >= order || reserve.Lap > lap)
                .OrderBy(h => h.Id);

        public static IQueryable<History> HistoriesAt(this GrimoireContext context, uint lap, uint order) =>
            context.Histories.Where(history => history.Lap == lap && history.Order == order)
                .OrderBy(h => h.Id);

        public static IIncludableQueryable<History, User> HistoriesList(this GrimoireContext context, uint lap, uint order) =>
            context.HistoriesAt(lap, order)
                .AsNoTracking()
                .Include(h => h.User);

        public static IIncludableQueryable<Report, User> ReportsList(this GrimoireContext context, uint lap, uint order) =>
            context.ReportsAt(lap, order)
                .AsNoTracking()
                .Include(h => h.User);
        
        public static async Task<EntityEntry<Report>> UpsertReport(this GrimoireContext context, uint lap, uint order, string comment, string userId)
        {
            var report = await context.Reports.FirstOrDefaultAsync(h =>
                h.Lap == lap & h.Order == order && h.UserId == userId);

            if (report != null)
                context.Reports.Remove(report);

            return await context.Reports.AddAsync(new Report
            {
                Lap = lap,
                Order = order,
                Comment = comment,
                UserId = userId
            });
        }
        
        public static async Task<Report> RemoveReport(this GrimoireContext context, uint lap, uint order, string userId)
        {
            var report = await context.Reports.FirstOrDefaultAsync(h =>
                h.Lap == lap & h.Order == order && h.UserId == userId);

            if (report != null)
                context.Reports.Remove(report);

            return report;
        }
    }
}