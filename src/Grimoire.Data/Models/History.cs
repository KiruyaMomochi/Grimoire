using System;
using Microsoft.EntityFrameworkCore;

namespace Grimoire.Data.Models
{
    [Index(nameof(Lap), nameof(Order))]
    public record History
    {
        public long Id { get; init; }
        
        public uint Lap { get; init; }
        public uint Order { get; init; }
        public string Comment { get; set; }

        public DateTimeOffset Time { get; init; }
        
        public bool HasRemainTime { get; init; }
        
        public bool IsRemain { get; init; }
        
        public string UserId { get; init; }
        public User User { get; init; }
    }
}