
using SocketClientServerLib.Extensions;
using SocketClientServerLib.Helpers;

namespace SocketClientServerLib.DTOs
{
    public record MessageRecord(
        DateTime timeStamp,
        Guid guidSender,
        IList<Guid>? receivers,
        Enum_CommandType commandType,
        string message)
    {
        public override string ToString()
            => $"[{timeStamp} {commandType.EnumCommandToString()} {guidSender}]: {message}\n";
        
    }
}
