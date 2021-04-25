using System.ComponentModel.DataAnnotations;

namespace Grimoire.Web.Models
{
    public record Group
    {
        [Key]
        public string GroupId { get; init; }
    }
}