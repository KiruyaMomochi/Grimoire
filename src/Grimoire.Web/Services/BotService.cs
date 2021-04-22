using System;
using System.Collections.Generic;
using Grimoire.LineApi.Source;
using Grimoire.Parser;
using isRock.LineBot;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Grimoire.Web.Services
{
    public class LineBotOptions
    {
        public const string LineBot = "LineBot";
         
        public string ChannelAccessToken { get; set; }
        public string Secret { get; set; }

        public Uri WebHook { get; set; }
    }

    public class BotService : IBotService
    {
        private readonly LineBotOptions _config;
        public isRock.LineBot.Bot Bot { get; }

        public BotService(IOptions<LineBotOptions> config)
        {
            _config = config.Value;

            var options = config.Value ?? throw new NullReferenceException(nameof(config));
            Bot = new Bot(options.ChannelAccessToken ??
                          throw new NullReferenceException(nameof(options.ChannelAccessToken)));
        }
        
        public LineUserInfo GetUserInfo(BaseSource source)
        {
            return source switch
            {
                GroupSource groupSource => Utility.GetGroupMemberProfile(groupSource.GroupId, groupSource.UserId,
                    _config.ChannelAccessToken),
                RoomSource roomSource => Utility.GetRoomMemberProfile(roomSource.RoomId, roomSource.UserId,
                    _config.ChannelAccessToken),
                UserSource userSource => Utility.GetUserInfo(userSource.UserId, _config.ChannelAccessToken),
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
            };
        }

        public void ReplyMessage(string replyToken, string message)
        {
            Bot.ReplyMessage(replyToken, message);
        }
    }
}