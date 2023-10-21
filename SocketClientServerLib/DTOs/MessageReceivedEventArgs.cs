
namespace SocketClientServerLib.DTOs
{
    public class MessageRecevedEventArgs : EventArgs
    {
        public MessageRecord Message { get; set; }
        public MessageRecevedEventArgs(MessageRecord message)
        {
            Message = message;
        }
    }
}
