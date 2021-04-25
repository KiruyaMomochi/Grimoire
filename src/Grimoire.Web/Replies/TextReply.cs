namespace Grimoire.Web.Replies
{
    public class TextReply
    {
        public TextReply(string message)
        {
            Message = message;
        }
        
        public string Message { get; }
    }
}