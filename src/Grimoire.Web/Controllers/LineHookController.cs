using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Grimoire.Line.Api.Webhook;
using Grimoire.Web.Services;
using isRock.LineBot;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TextMessage = Grimoire.Line.Api.Webhook.Message.TextMessage;

namespace Grimoire.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LineHookController : ControllerBase
    {
        private readonly ILogger<LineHookController> _logger;

        // private readonly IUpdateService _updateService;
        private readonly CommandManager _manager;
        private readonly IBotService _botService;

        public LineHookController(
            ILogger<LineHookController> logger, CommandManager commandManager, IBotService botService)
        {
            _logger = logger;
            _manager = commandManager;
            _botService = botService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(WebhookEvent we)
        {
            foreach (var e in we.Events)
                await _manager.HandleWebhookEvent(e);

            return Ok();
        }
    }
}