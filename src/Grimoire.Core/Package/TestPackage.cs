using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Grimoire.Data;
using Grimoire.Data.Models;
using Grimoire.Explore;
using Grimoire.Line.Api.Webhook.Event;
using Grimoire.Line.Api.Webhook.Source;
using Microsoft.Extensions.Logging;

namespace Grimoire.Core.Package
{
    public class TestPackage : PackageBase
    {
        private readonly ILogger<TestPackage> _logger;
        private readonly GrimoireDatabaseContext _context;

        public TestPackage(ILogger<TestPackage> logger, GrimoireDatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Command("ping", "Ping")]
        public async Task<string> Ping()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Pong.");

            var userId = Event.Source.UserId;
            sb.Append("UserId: ").AppendLine(userId);
            var admin = await _context.Admins.FindAsync(userId);
            if (admin != null)
                sb.AppendLine(" - You are admin.");
            else
                sb.AppendLine(" - You are not admin.");

            if (Event.Source is not GroupSource groupSource)
                return sb.ToString().TrimEnd();

            var groupId = groupSource.GroupId;
            sb.Append("GroupId: ").AppendLine(groupId);
            var allow = await _context.Groups.FindAsync(groupId);
            if (allow != null)
                sb.AppendLine(" - This group is allowed.");
            else
                sb.AppendLine(" - This group is not allowed.");

            return sb.ToString().TrimEnd();
        }

        [Command("raw")]
        public string Raw()
        {
            return JsonSerializer.Serialize(Event,
                    new JsonSerializerOptions {WriteIndented = true});
        }
    }
}