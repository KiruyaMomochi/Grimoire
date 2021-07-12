using System.ComponentModel.DataAnnotations;

namespace Grimoire.Data.Models
{
    public record Group
    {
        [Key]
        public string GroupId { get; init; }
    }
}