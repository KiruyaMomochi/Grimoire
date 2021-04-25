using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Grimoire.LineApi;
using Grimoire.LineApi.Event;
using Grimoire.Web.Services;
using isRock.LineBot;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TextMessage = Grimoire.LineApi.Message.TextMessage;

namespace Grimoire.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LineHookController : ControllerBase
    {
        private readonly ILogger<LineHookController> _logger;
        // private readonly IUpdateService _updateService;
        private readonly CommandManager _manager;

        public LineHookController(
            ILogger<LineHookController> logger, CommandManager commandManager)
        {
            _logger = logger;
            _manager = commandManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post(WebhookEvent we)
       {
            if (!Request.Headers.TryGetValue("x-line-signature", out var signature))
                return BadRequest("Signature not found");
            
            _logger.LogWarning("x-line-signature: {Signature}", signature);
            // TODO: Signature verification
            
            foreach (var e in we.Events) 
                await _manager.HandleWebhookEvent(e);
            
            return Ok();
        }
    }
}