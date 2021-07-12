using System.Threading.Tasks;
using Grimoire.Core.Services;
using Grimoire.Data;
using Grimoire.Data.Models;
using Grimoire.Explore;
using Grimoire.Line.Api.Webhook.Source;
using Microsoft.Extensions.Logging;

namespace Grimoire.Core.Package
{
    public class AdminPackage: PackageBase
    {
        private readonly ILogger<AdminPackage> _logger;
        private readonly GrimoireDatabaseContext _context;
        private readonly UsernameService _usernameService;
        private readonly IBotService _botService;

        public AdminPackage(ILogger<AdminPackage> logger, UsernameService usernameService, IBotService botService,
            GrimoireDatabaseContext context)
        {
            _logger = logger;
            _context = context;
            _usernameService = usernameService;
            _botService = botService;
        }

        [GroupCommand("add")]
        public async Task<string> AddGroup()
        {
            var source = (GroupSource) Event.Source;
            var a = await _context.Admins.FindAsync(source.UserId);
            if (a == null)
                return $"You are not admin, so you can't use this.\nYour user id is {source.UserId}.";
            
            var g = await _context.Groups.FindAsync(source.GroupId);
            if (g != null)
                return "This group is already in the allow list.";

            await _context.Groups.AddAsync(new Group() {GroupId = source.GroupId});
            await _context.SaveChangesAsync();
            return "Done. Use #ping to validate.";
        }
    }
}
