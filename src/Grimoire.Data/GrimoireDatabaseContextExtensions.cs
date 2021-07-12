using System.Linq;
using System.Threading.Tasks;
using Grimoire.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;

namespace Grimoire.Data
{
    public static class GrimoireDatabaseContextExtensions
    {
        public static IQueryable<Report> ReportsAt(this GrimoireDatabaseContext databaseContext, uint lap, uint order) =>
            databaseContext.Reports.Where(reserve => reserve.Lap == lap && reserve.Order == order)
                .OrderBy(h => h.Id);

        public static IQueryable<Report> ReportsAfter(this GrimoireDatabaseContext databaseContext, uint lap, uint order) =>
            databaseContext.Reports.Where(reserve => reserve.Lap == lap && reserve.Order >= order || reserve.Lap > lap)
                .OrderBy(h => h.Id);

        public static IQueryable<History> HistoriesAt(this GrimoireDatabaseContext databaseContext, uint lap, uint order) =>
            databaseContext.Histories.Where(history => history.Lap == lap && history.Order == order)
                .OrderBy(h => h.Id);

        public static IIncludableQueryable<History, User> HistoriesList(this GrimoireDatabaseContext databaseContext, uint lap, uint order) =>
            databaseContext.HistoriesAt(lap, order)
                .AsNoTracking()
                .Include(h => h.User);

        public static IIncludableQueryable<Report, User> ReportsList(this GrimoireDatabaseContext databaseContext, uint lap, uint order) =>
            databaseContext.ReportsAt(lap, order)
                .AsNoTracking()
                .Include(h => h.User);
        
        public static async Task<EntityEntry<Report>> UpsertReport(this GrimoireDatabaseContext databaseContext, uint lap, uint order, string comment, string userId)
        {
            var report = await databaseContext.Reports.FirstOrDefaultAsync(h =>
                h.Lap == lap & h.Order == order && h.UserId == userId);

            if (report != null)
                databaseContext.Reports.Remove(report);

            return await databaseContext.Reports.AddAsync(new Report
            {
                Lap = lap,
                Order = order,
                Comment = comment,
                UserId = userId
            });
        }
        
        public static async Task<Report> RemoveReport(this GrimoireDatabaseContext databaseContext, uint lap, uint order, string userId)
        {
            var report = await databaseContext.Reports.FirstOrDefaultAsync(h =>
                h.Lap == lap & h.Order == order && h.UserId == userId);

            if (report != null)
                databaseContext.Reports.Remove(report);

            return report;
        }
    }
}