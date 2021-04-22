using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grimoire.Web.Models
{
    public record Admin
    {
        public int Id { get; init; }
        public string UserId { get; init; }
        public User User { get; init; }
    }
}