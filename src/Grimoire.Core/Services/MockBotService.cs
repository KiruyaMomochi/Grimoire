using System;
using System.Collections.Generic;
using System.IO;
using Grimoire.Line.Api.Webhook.Source;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Grimoire.Core.Services
{
    public class MockBotService : IBotService
    {
        public readonly Dictionary<string, string> Usernames = new();
        private readonly ILogger<MockBotService> _logger;

        public MockBotService(IOptions<LineBotOptions> config, ILogger<MockBotService> logger)
        {
            _logger = logger;
        }
        
        public string GetUsername(BaseSource source)
        {
            return "displayName";
        }

        public bool TryGetUsername(string userId, out string username) 
            => Usernames.TryGetValue(userId, out username);

        public string SetUsernameCache(string userId, string username) 
            => Usernames[username] = username;

        public void ReplyMessage(string replyToken, string message)
        {
            _logger.LogInformation("Reply {Message} to {Token}", message, replyToken);
        }

        public bool ValidateSignature(Stream stream, ReadOnlySpan<byte> remoteSignature)
        {
            return true;
        }

        public string FetchUsername(BaseSource source)
        {
            var userInfo = GetUsername(source);
            Usernames[source.UserId] = userInfo;
            return userInfo;
        }
    }
}