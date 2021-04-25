using System;
using System.IO;
using System.Threading.Tasks;
using Grimoire.LineApi.Source;
using isRock.LineBot;

namespace Grimoire.Web.Services
{
    public interface IBotService
    {
        LineUserInfo GetUserInfo(BaseSource source);
        void ReplyMessage(string replyToken, string message);
        bool ValidateSignature(Stream stream, ReadOnlySpan<byte> remoteSignature);
    }
}