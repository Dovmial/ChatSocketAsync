using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text;
using SocketClientServerLib.DTOs;
using SocketClientServerLib.Helpers;

namespace SocketClientServerLib.Extensions
{
    internal static class Extensions
    {
        internal static string EnumCommandToString(this Enum_CommandType command) =>
            command switch
            {
                Enum_CommandType.NEW_GUID => "<new-guid>",
                Enum_CommandType.GET_HISTORY => "<get-all-messagies>",
                Enum_CommandType.LOG_IN => "<log-in>",
                Enum_CommandType.LOG_OUT => "<log-out>",
                Enum_CommandType.REGISTRATION => "<registration>",
                Enum_CommandType.SEND_TO => "<send-to>",
                Enum_CommandType.SEND_TO_ALL => "<send-to-all>",
                Enum_CommandType.SEND_TO_OTHER => "<send-to-other>",
                Enum_CommandType.SEND_TO_SERVER => "<send-to-server>",
                Enum_CommandType.INFO_FROM_SERVER | Enum_CommandType.CLIENT_INVITE => "<client-invite-fromServer>",
                Enum_CommandType.INFO_FROM_SERVER => "<info-from-server>",
                Enum_CommandType.INFO_FROM_SERVER | Enum_CommandType.LOG_IN => "<error-login>",
                Enum_CommandType.INFO_FROM_SERVER | Enum_CommandType.LOG_OUT => "<log-out-client>",
                Enum_CommandType.INFO_FROM_SERVER | Enum_CommandType.GET_HISTORY => "<all_messagies>",
                _ => throw new NotImplementedException($"Неизвестный тип операции: {command}")
            };
        internal static TaskAwaiter GetAwaiter(this TimeSpan timeSpan) => Task.Delay(timeSpan).GetAwaiter();
        internal static TimeSpan sec(this int seconds) => TimeSpan.FromSeconds(seconds);
        internal static TimeSpan ms(this int ms) => TimeSpan.FromMilliseconds(ms);
        internal static byte[] GetBytesJson(this MessageRecord message, Encoding encoding)
            => encoding.GetBytes(JsonSerializer.Serialize(message));
    }
}
