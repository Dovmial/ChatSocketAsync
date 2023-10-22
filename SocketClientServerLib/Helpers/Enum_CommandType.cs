
namespace SocketClientServerLib.Helpers
{
    [Flags]
    public enum Enum_CommandType: short
    {
        None = 0,
        INFO_FROM_SERVER = 1,
        LOG_IN = 2,
        LOG_OUT = 4,
        CLIENT_INVITE = 8,
        REGISTRATION = 16,
        NEW_GUID = 32,
        SEND_TO = 64,
        SEND_TO_SERVER = 128,
        SEND_TO_OTHER = 256,
        SEND_TO_ALL = 512,
        GET_HISTORY = 1024,
    }
}
