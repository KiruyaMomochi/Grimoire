using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Grimoire.Web.Models
{
    public class GrimoireContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Current> Currents { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Group> Groups { get; set; }

        public GrimoireContext(DbContextOptions<GrimoireContext> options) : base(options)
        {
        }

        public async Task<Current> CurrentAsync() => await Currents.OrderByDescending(c => c.Id).FirstOrDefaultAsync();
        public async Task<Current> CurrenNoTrackingtAsync() => await Currents.AsNoTracking().OrderByDescending(c => c.Id).FirstOrDefaultAsync();
        public Current Current() => Currents.Find(1);

    }
}