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
    public class AdminSystem: CommandBase
    {
        private readonly ILogger<AdminSystem> _logger;
        private readonly GrimoireContext _context;
        private readonly UsernameService _usernameService;
        private readonly IBotService _botService;

        public AdminSystem(ILogger<AdminSystem> logger, UsernameService usernameService, IBotService botService,
            GrimoireContext context)
        {
            _logger = logger;
            _context = context;
            _usernameService = usernameService;
            _botService = botService;
        }

        [GroupCommand("add")]
        public async Task<TextReply> AddGroup()
        {
            var source = (GroupSource) MessageEvent.Source;
            var a = await _context.Admins.FindAsync(source.UserId);
            if (a == null)
                return new TextReply($"You are not admin, so you can't use this.\nYour user id is {source.UserId}.");
            
            var g = await _context.Groups.FindAsync(source.GroupId);
            if (g != null)
                return new TextReply("This group is already in the allow list.");

            await _context.Groups.AddAsync(new Group() {GroupId = source.GroupId});
            await _context.SaveChangesAsync();
            return new TextReply("Done. Use #ping to validate.");
        }
    }
}
