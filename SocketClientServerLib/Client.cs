

using SocketClientServerLib.DTOs;
using SocketClientServerLib.Extensions;
using SocketClientServerLib.Helpers;
using SocketClientServerLib.Interfaces;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace SocketClientServerLib
{
    public sealed class Client : SocketBase
    {
        public bool IsConnected => _socketBase?.Connected ?? false;
        public IList<UserDTO>? UserList { get; private set; }
        public Guid? ServerGuid { get; private set; }
        private event EventHandler<MessageRecevedEventArgs> MessageReceived;
        public IResponseHandler ResponseHandler { get; private set; }

        public Client(
            Encoding encoding,
            Logger logger,
            IResponseHandler responseHandler,
            CancellationToken token) : base(encoding, logger, token)
        {
            UserList = new List<UserDTO>();
            ResponseHandler = responseHandler;
            MessageReceived += OnMessageReceived;
        }

        public async Task<bool> ConnectAsync(string? serverIP, string? serverPort)
        {
            if (IsConnected)
            {
                _logger.WritelineError("Уже подключен");
                return false;
            }
            if (IPEndPoint.TryParse($"{serverIP}:{serverPort}", out IPEndPoint? endPoint) == false)
            {
                _logger.WritelineError("Некорректный адрес или порт сокета");
                return false;
            }
            if (_socketBase is null)
                InitSocket();
            try
            {
                await _socketBase!.ConnectAsync(endPoint, _token);
                string message = _socketBase.Connected ? "Соединение установлено" : "Нет соединения";
                _logger.Writeline(message);
                MessageRecord responseServer = await ReceiveDataAsync(_socketBase);
                _logger.Writeline($"{responseServer.message} [guid сервера]");
                if (Guid.TryParse(responseServer.message, out Guid guidServer))
                {
                    UserList!.Add(new(guidServer, "сервер", true));
                    ServerGuid = guidServer;
                }
                return true;
            }
            catch (OperationCanceledException ex)
            {
                _logger.WritelineError(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                _logger.WritelineError(ex.Message);
                return false;
            }
        }
        public async Task DisconnectAsync()
        {
            await SendAsync(new(
                DateTime.Now,
                Guid,
                null,
                Enum_CommandType.LOG_OUT,
                string.Empty),
                _socketBase!);

            _socketBase!.Shutdown(SocketShutdown.Both);
            _socketBase.Close();

            Dispose();
        }
        public async Task ReceivingMessageStart()
        {
            try
            {
                while (_token.IsCancellationRequested == false)
                {
                    if (IsConnected == false)
                    {
                        _logger.WritelineError("socket is not connection");
                        return;
                    }
                    MessageRecord record = await ReceiveDataAsync(_socketBase!);
                    //send event
                    MessageReceived?.Invoke(this, new(record));
                }
            }
            catch (SocketException ex)
            {
                _logger.WritelineError($"socket: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                _logger.WritelineError($"cancelToken: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.WritelineError(ex.Message);
            }
            finally
            {
                await DisconnectAsync();
            }
        }

        public override async Task SendAsync(MessageRecord message, Socket? clientSocket = null)
        {
            Socket s = clientSocket ?? _socketBase!;
            await s.SendToAsync(
                message.GetBytesJson(_encoding),
                SocketFlags.None,
                s.RemoteEndPoint!,
                _token);
        }
        public async Task SendToAsync(UserDTO? selectedClient, string textToSend)
        {
            if (IsConnected == false)
            {
                _logger.WritelineError("Клиент не подключен к серверу");
                return;
            }
            if (selectedClient is null)
            {
                _logger.WritelineError("Адресат не выбран");
                return;
            }
            try
            {
                await SendAsync(new(
                    DateTime.Now,
                    Guid,
                    new List<Guid>() { (Guid)selectedClient?.guid! },
                    Enum_CommandType.SEND_TO,
                    textToSend));
            }
            catch (SocketException ex)
            {
                _logger.WritelineError($"socket: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.WritelineError(ex.Message);
            }
        }
        public async Task Registration(string login, string password, string name)
        {
            _logger.Writeline("try registration..");
            await SendAsync(new(
                 DateTime.Now,
                Guid,
                null,
                Enum_CommandType.REGISTRATION,
                $"{login}{(char)29}{password}{(char)29}{name}"
                ));
        }
        public async Task LogIn(string login, string password, string name)
        {
            
            _logger.Writeline("try authentification..");
            await SendAsync(new(
                DateTime.Now,
                Guid,
                null,
                Enum_CommandType.LOG_IN,
                $"{login}{(char)29}{password}{(char)29}{name}"
                ));
        }
        private async void OnMessageReceived(object? sender, MessageRecevedEventArgs e)
        {
            Task task = e.Message.commandType switch
            {
                Enum_CommandType.NEW_GUID => RegistrationHandle(e.Message.message),
                (Enum_CommandType.INFO_FROM_SERVER | Enum_CommandType.LOG_IN) => LoginResponseError(e.Message),
                Enum_CommandType.LOG_IN => LoginAccess(e.Message),
                Enum_CommandType.SEND_TO => SendToHandle(e.Message),
                Enum_CommandType.INFO_FROM_SERVER | Enum_CommandType.CLIENT_INVITE => Invite(e.Message),
                Enum_CommandType.INFO_FROM_SERVER | Enum_CommandType.LOG_OUT => LogOut(e.Message),
                _ => throw new NotImplementedException("Неизвестный тип команды")
            };
            await task;

            Task Invite(MessageRecord record)
            {
                string[] parts = record.message.Split((char)29); //name + guid
                if (Guid.TryParse(parts[1], out Guid guid))
                {
                    UserDTO user = new(guid, parts[0], true);
                    UserList!.Add(user);
                    ResponseHandler.ClientInviteHandle(user); 
                }
                else
                    _logger.WritelineError("Unknown user");
                return Task.CompletedTask;
            }
            Task LogOut(MessageRecord record)
            {
                _logger.Writeline(record.ToString());
                if (Guid.TryParse(record.message, out Guid guid) == false)
                    return Task.CompletedTask;
                UserDTO? user = UserList?.FirstOrDefault(u => u.guid == guid);
                ResponseHandler.ClientLogOutHandle(user);
                return Task.CompletedTask;
            }
            Task LoginAccess(MessageRecord record)
            {
                _logger.Writeline((record with { message = "Authentication success" }).ToString());
                //список онлайн клиентов
                ReadOnlySpan<byte> buffer = new(Convert.FromBase64String(record.message));
                UserDTO[] usersOnline = JsonSerializer.Deserialize<UserDTO[]>(buffer)!;
                foreach (UserDTO user in usersOnline)
                    UserList!.Add(user);
                ResponseHandler.LoginAccessHandle(usersOnline);
                return Task.CompletedTask;
            }
            Task LoginResponseError(MessageRecord record)
            {
                _logger.Writeline(record.ToString());
                return Task.CompletedTask;
            }
            Task SendToHandle(MessageRecord message)
            {
                _logger.Writeline(message.ToString());
                return Task.CompletedTask;
            }
            async Task RegistrationHandle(string guid)
            {
                if (Guid.TryParse(guid, out Guid g))
                    Guid = g;
                (bool result, string error) = await ResponseHandler.RegistrationResponseHandle(guid);
                if (result == false)
                {
                    _logger.WritelineError(error);
                    return;
                }
                _logger.Writeline("Registry is successfull");
            }
        }


    }
}
