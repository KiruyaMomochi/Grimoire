using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            if (!Request.Headers.TryGetValue("x-line-signature", out var signatureValues))
                return BadRequest("Signature not found");
            
            var result = _botService.ValidateSignature(Request.Body, Convert.FromBase64String(signatureValues));
            if (!result)
            {
                _logger.LogWarning("Signature validation failed");
                return BadRequest("Signature validation failed");
            }

            foreach (var e in we.Events) 
                await _manager.HandleWebhookEvent(e);
            
            return Ok();
        }
    }
}