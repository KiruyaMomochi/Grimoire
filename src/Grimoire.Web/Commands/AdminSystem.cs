using System.Text;
using Grimoire.Web.Models;
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
        
        
    }
}