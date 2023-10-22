
using DataStorageLayer.Models;
using SocketClientServerLib;
using SocketClientServerLib.DTOs;
using SocketClientServerLib.Interfaces;
using System.Text.Json;

namespace SocketClientWF.Helpers
{
    internal class ResponseServerHandler : IResponseHandler
    {
        internal Action<UserDTO> InviteAction {  get; set; }
        internal Action<UserDTO?> LogoutAction {  get; set; }
        internal Action<UserDTO[]> LoginAccessAction {  get; set; }
        internal Func<string, Task<(bool result, string error)>> RegistrationAction {  get; set; }
        internal Action<string> ReceiveHistoryAction { get; set; }
        public ResponseServerHandler(
            Action<UserDTO> inviteAction,
            Action<UserDTO?> logoutAction,
            Action<UserDTO[]> loginAccessAction, 
            Func<string, Task<(bool result, string error)>> registration,
            Action<string> receiveHistoryAction)
        {
            InviteAction = inviteAction;
            LogoutAction = logoutAction;
            LoginAccessAction = loginAccessAction;
            RegistrationAction = registration;
            ReceiveHistoryAction = receiveHistoryAction;
        }
        public void ClientInviteHandle(UserDTO user) => InviteAction(user);
        public void ClientLogOutHandle(UserDTO? userToLogout) => LogoutAction(userToLogout);
        public void LoginAccessHandle(UserDTO[] usersOnline) => LoginAccessAction(usersOnline);
        public Task<(bool, string)> RegistrationResponseHandle(string guid) => RegistrationAction(guid);
        public void ShowHistoryMessages(string messagiesList) => ReceiveHistoryAction(messagiesList);
    }
}
