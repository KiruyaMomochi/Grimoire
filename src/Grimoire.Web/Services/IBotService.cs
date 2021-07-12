using System;
using System.IO;
using System.Threading.Tasks;
using Grimoire.Line.Api.Webhook.Source;
using isRock.LineBot;

namespace Grimoire.Web.Services
{
    public interface IBotService
    {
        string GetUserInfo(BaseSource source);
        void ReplyMessage(string replyToken, string message);
    }
}