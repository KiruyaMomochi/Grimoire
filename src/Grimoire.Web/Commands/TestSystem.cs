using System.Text;
using System.Threading.Tasks;
using Grimoire.LineApi.Source;
using Grimoire.Web.Builder;
using Grimoire.Web.Models;
using Grimoire.Web.Replies;
using Grimoire.Web.Services;
using Microsoft.Extensions.Logging;

namespace Grimoire.Web.Commands
{
    public class TestSystem: CommandBase
    {
        private readonly ILogger<TestSystem> _logger;
        private readonly GrimoireContext _context;

        public TestSystem(ILogger<TestSystem> logger, GrimoireContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Command("ping", "Ping")]
        public async Task<TextReply> Ping()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Pong.");
            
            var userId = MessageEvent.Source.UserId;
            sb.Append("UserId: ").AppendLine(userId);
            var admin = await _context.Admins.FindAsync(userId);
            if (admin != null)
                sb.AppendLine(" - You are admin.");
            else
                sb.AppendLine(" - You are not admin.");

            if (MessageEvent.Source is not GroupSource groupSource) 
                return new TextReply(sb.ToString().TrimEnd());
            
            sb.Append("GroupId: ").AppendLine(groupSource.GroupId);
            var allow = await _context.Groups.FindAsync(groupSource);
            if (allow != null)
                sb.AppendLine(" - This group is allowed.");
            else
                sb.AppendLine(" - This group is not allowed.");

            return new TextReply(sb.ToString().TrimEnd());
        }
    }
}