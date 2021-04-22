using Grimoire.LineApi.Source;
using Grimoire.Parser;
using isRock.LineBot;

namespace Grimoire.Web.Services
{
    public interface IBotService
    {
        LineUserInfo GetUserInfo(BaseSource source);
        void ReplyMessage(string replyToken, string message);
    }
}