using Grimoire.Line.Api.Webhook.Source;

namespace Grimoire.Core.Services
{
    public interface IBotService
    {
        string GetUsername(BaseSource source);
        void ReplyMessage(string replyToken, string message);
    }
}