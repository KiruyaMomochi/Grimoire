using System.Collections.Generic;

namespace Grimoire.Data.Models
{
    public record User
    {
        public string UserId { get; init; }
        public string LineName { get; set; }
        public string PriconneName { get; set; }

        public List<Report> Reports { get; set; }
        public List<History> Histories { get; set; }
    }
}
