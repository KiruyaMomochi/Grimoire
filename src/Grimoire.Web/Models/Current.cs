using System;

namespace Grimoire.Web.Models
{
    public record Current
    {
        public int Id { get; init; }
        public uint Lap { get; set; }
        public uint Order { get; set; }

        public static Current Default = new() {Id = 1, Lap = 1, Order = 1};

        public void Advance(int delta)
        {
            var o = delta + Order - 1;
            var l = Lap + o / 5;
            o %= 5;
            if (o < 0)
            {
                o += 5;
                l -= 1;
            }

            o += 1;

            if (l <= 0 || o <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(delta), "Both lap and order should larger than zero.");
            Lap = (uint) l;
            Order = (uint) o;
        }

        public void Update(uint? lap, uint? order)
        {
            if (lap.HasValue) Lap = lap.Value;
            if (order.HasValue) Order = order.Value;
        }
    }
}