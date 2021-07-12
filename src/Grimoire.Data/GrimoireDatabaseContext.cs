using System.Linq;
using System.Threading.Tasks;
using Grimoire.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Grimoire.Data
{
    public class GrimoireDatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Current> Currents { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Group> Groups { get; set; }

        public GrimoireDatabaseContext(DbContextOptions<GrimoireDatabaseContext> options) : base(options)
        {
        }

        public async Task<Current> CurrentAsync() => await Currents.OrderByDescending(c => c.Id).FirstOrDefaultAsync();
        public async Task<Current> CurrentNoTrackingAsync() => await Currents.AsNoTracking().OrderByDescending(c => c.Id).FirstOrDefaultAsync();
        public Current Current() => Currents.Find(1);

    }
}