using System;
using System.Collections.Generic;
using Grimoire.LineApi.Source;
using isRock.LineBot;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Grimoire.Web.Services
{
    public class MockBotService : IBotService
    {
        public readonly Dictionary<string, string> Usernames = new();
        private readonly ILogger<MockBotService> _logger;

        public MockBotService(IOptions<LineBotOptions> config, ILogger<MockBotService> logger)
        {
            _logger = logger;
        }
        
        public LineUserInfo GetUserInfo(BaseSource source)
        {
            return new (){displayName = "displayName"};
        }

        public bool TryGetUsername(string userId, out string username) 
            => Usernames.TryGetValue(userId, out username);

        public string SetUsernameCache(string userId, string username) 
            => Usernames[username] = username;

        public void ReplyMessage(string replyToken, string message)
        {
            _logger.LogInformation("Reply {Message} to {Token}", message, replyToken);
        }

        public string FetchUsername(BaseSource source)
        {
            var userInfo = GetUserInfo(source);
            Usernames[source.UserId] = userInfo.displayName;
            return userInfo.displayName;
        }
    }
}