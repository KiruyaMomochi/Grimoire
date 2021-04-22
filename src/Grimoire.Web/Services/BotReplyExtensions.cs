namespace Grimoire.Web.Services
{
    public static class BotReplyExtensions
    {
        public static void ReplyCurrentNotSet(this IBotService botService, string replyToken)
        {
            botService.ReplyMessage(replyToken, "未設定當前王，請使用 #切 設定後再試。");;
        }
    }
}