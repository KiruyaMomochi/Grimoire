using System.ComponentModel.DataAnnotations;

namespace Grimoire.Data.Models
{
    public record Admin
    {
        [Key]
        public string UserId { get; init; }
        public User User { get; init; }
    }
}