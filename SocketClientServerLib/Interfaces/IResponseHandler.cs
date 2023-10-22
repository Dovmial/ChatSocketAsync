
using SocketClientServerLib.DTOs;

namespace SocketClientServerLib.Interfaces
{
    public interface IResponseHandler
    {
        Task<(bool, string)> RegistrationResponseHandle(string guid);
        void LoginAccessHandle(UserDTO[] usersOnline);
        void ClientInviteHandle(UserDTO user);
        void ClientLogOutHandle(UserDTO? userToLogout);
        void ShowHistoryMessages(string messagiesList);
    }
}
