using Microsoft.EntityFrameworkCore;

namespace Grimoire.Web.Models
{
    [Index(nameof(Lap), nameof(Order))]
    public record Report
    {
        public int Id { get; init; }

        public uint Lap { get; init; }
        public uint Order { get; init; }
        public string Comment { get; set; }

        public string UserId { get; init; }
        public User User { get; init; }
        
        public bool IsFailed { get; set; }
    }
}