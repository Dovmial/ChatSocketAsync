using DataStorageLayer;
using DataStorageLayer.Models;
using SocketClientServerLib.DTOs;
using SocketClientServerLib.Extensions;
using SocketClientServerLib.Helpers;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace SocketClientServerLib
{
    public sealed class Listener : SocketBase
    {
        private readonly int _poolSizeClients;
        private Dictionary<Guid, Socket> _guidSockets;
        private IHasher _hasher;

        //Храним только подключенных пользователей
        public Listener(
            int poolSizeClients,
            Encoding encoding,
            Logger logger,
            CancellationToken token) : base(encoding, logger, token)
        {
            InitSocket();
            _poolSizeClients = poolSizeClients;
            _guidSockets = new();
            Guid = Guid.NewGuid();
            _hasher = new Cryptographer();
        }

        /// <summary>
        /// запуск сервера
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public async Task StartAsync(int port)
        {
            try
            {
                if (_socketBase is null)
                    InitSocket();
                _socketBase!.Bind(new IPEndPoint(IPAddress.Any, port));
                _socketBase.Listen(_poolSizeClients);
                _logger.Writeline("Server started...");
                _ = Task.Run(async () =>
                {
                    while (_token.IsCancellationRequested == false)
                    {
                        _logger.Writeline(DateTime.Now.ToString());
                        await 3.sec();
                    }
                });
                await AcceptClientAsync();
            }
            catch (SocketException ex)
            {
                _logger.WritelineError($"socket: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.WritelineError(ex.Message);
            }
            finally
            {
                RemoveConnection();
            }

        }

        /// <summary>
        /// прием подключений
        /// </summary>
        /// <returns></returns>
        private async Task AcceptClientAsync()
        {
            _logger.Writeline("Ready to connections.");
            try
            {
                while (_token.IsCancellationRequested == false)
                {
                    Socket clientSocket = await _socketBase!.AcceptAsync(_token);
                    _logger.Writeline($"{DateTime.Now}: Подключился {clientSocket.RemoteEndPoint}");

                    //отправка guid сервера
                    await SendAsync(new(
                        DateTime.Now,
                        Guid,
                        null,
                        Enum_CommandType.INFO_FROM_SERVER,
                        Guid.ToString()),
                        clientSocket);
                    //бесконечные прием и обработка сообщений.
                    _ = Task.Run(async () =>
                    {
                        Guid guid = default;
                        try
                        {
                            await ProcessSocketHandlerAsync(clientSocket);
                        }
                        catch (SocketException ex)
                        {
                            guid = _guidSockets.FirstOrDefault(x => x.Value == clientSocket).Key;
                            _logger?.WritelineError($"{DateTime.Now} socket [{clientSocket.RemoteEndPoint} -- {guid}]: {ex.Message}");
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
                            if(guid == default)
                                guid = _guidSockets.FirstOrDefault(x => x.Value == clientSocket).Key;
                           
                            RemoveConnectionClient(clientSocket, guid);
                            await SendToAll(new(
                                DateTime.Now,
                                Guid,
                                null,
                                Enum_CommandType.LOG_OUT | Enum_CommandType.INFO_FROM_SERVER,
                                guid.ToString()
                                ));
                        }
                    });
                }
            }
            catch (SocketException ex)
            {
                _logger?.WritelineError($"socket: {ex.Message}");
            }
            catch (OperationCanceledException ex)
            {
                _logger.WritelineError($"cancelToken: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.WritelineError(ex.Message);
            }
        }

        /// <summary>
        /// обработка подключенного сокета
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task ProcessSocketHandlerAsync(Socket clientSocket)
        {
            MessageRecord? request;
            MessageRecord? response;
            while (_token.IsCancellationRequested == false)
            {
                request = await ReceiveDataAsync(clientSocket);
                if (request is null)
                    continue;

                //does not flash the password at the server's log
                if (request.commandType is Enum_CommandType.REGISTRATION or Enum_CommandType.LOG_IN)
                    _logger.Writeline((request with { message = string.Empty }).ToString());
                else
                    _logger.Writeline(request.ToString() ?? "???");

                response = CreateResponse(request, clientSocket);
                Task task = response.commandType switch
                {
                    Enum_CommandType.NEW_GUID => SendAsync(response, clientSocket),
                    Enum_CommandType.SEND_TO => SendAsync(response, _guidSockets[response.receivers![0]]),
                    Enum_CommandType.SEND_TO_OTHER => SendToOtherBroadcastAsync(response, request.guidSender),
                    Enum_CommandType.SEND_TO_ALL => SendToAll(response),
                    Enum_CommandType.SEND_TO_SERVER or
                    Enum_CommandType.INFO_FROM_SERVER | Enum_CommandType.LOG_IN => SendAsync(response, clientSocket),
                    Enum_CommandType.INFO_FROM_SERVER | Enum_CommandType.CLIENT_INVITE => LogInHandle(response, clientSocket),

                    _ => throw new NotImplementedException("Неизвестный тип команды")
                };
                await task;
            }
        }

        #region  Ответы сервера
        private async Task SendToOtherBroadcastAsync(MessageRecord message, Guid guid)
        {
            foreach (var user in _guidSockets)
                if (user.Key != guid)
                    await SendAsync(message, _guidSockets[user.Key]);
        }
        private async Task SendToAll(MessageRecord message)
        {
            foreach (var user in _guidSockets)
                await SendAsync(message, user.Value);
        }
        private async Task LogInHandle(MessageRecord message, Socket client)
        {
            //Users to send
            Guid guid = _guidSockets.FirstOrDefault(x => x.Value == client).Key;

            using DbGateway db = new();
            List<UserDTO> users = db.GetAllUser()
                .Where(x => x.IsOnline == true && x.Guid != guid)
                .Select(x => new UserDTO(x.Guid, x.Name ?? string.Empty, x.IsOnline))
                .ToList();
            string usersBase64 = Convert.ToBase64String(JsonSerializer.SerializeToUtf8Bytes(users));

            await SendAsync(message with { commandType = Enum_CommandType.LOG_IN, message = $"{usersBase64}" }, client);
            await SendToOtherBroadcastAsync(message, guid);
        }
        private void RemoveConnectionClient(Socket socketUser, Guid guid)
        {
            //изменение статуса в базе
            using DbGateway db = new();
            User user = db.GetUser(guid)!;
            user.IsOnline = false;
            user.LastLogin = DateTime.Now;
            db.Update(user);
            db.SaveChanges();
            
            socketUser.Shutdown(SocketShutdown.Both);
            socketUser.Close();
            _guidSockets.Remove(guid);
        }
        private MessageRecord CreateResponse(MessageRecord message, Socket userSocket)
            => message.commandType switch
            {
                Enum_CommandType.REGISTRATION => Registration(message, userSocket),
                Enum_CommandType.LOG_OUT => LogOut(message),
                Enum_CommandType.LOG_IN => LogIn(message, userSocket),
                Enum_CommandType.SEND_TO or
                Enum_CommandType.SEND_TO_SERVER => SendTo(message),
                _ => throw new NotImplementedException()
            };
        #endregion

        #region Обработка запросов
        private MessageRecord Registration(MessageRecord message, Socket userSocket)
        {
            UserAuthDataDTO userAuth = UserAuthDataDTO.CreateFromString(message.message);
            //TODO: регистрация через почту
            Guid guid = Guid.NewGuid();

            _logger.Writeline($"New user is registry.. {guid}");

            //если пользователь не указал имя, то оно будет guid
            string name = string.IsNullOrWhiteSpace(userAuth.name)
                ? guid.ToString()
                : userAuth.name;

            //запись в базу
            using DbGateway db = new();
            User newUser = new()
            {
                Guid = guid,
                Login = userAuth.login,
                HashedPassword = _hasher.Hash(userAuth.pass),
                Name = name,
                IsOnline = false //true при авторизации
            };
            _ = db.Add(newUser);
            db.SaveChanges();
            _guidSockets.Add(guid, userSocket);
            return new MessageRecord
            (
                commandType: Enum_CommandType.NEW_GUID,
                guidSender: Guid,
                message: guid.ToString(), 
                receivers: null,
                timeStamp: DateTime.Now
            );
        }

        private MessageRecord LogOut(MessageRecord request)
        {
            _logger.Writeline($"Пользователь {request.guidSender} - покинул чат");
            return new
            (
                guidSender: Guid,
                commandType: Enum_CommandType.SEND_TO_OTHER,
                message: $"{request.guidSender} - покинул чат",
                receivers: null,
                timeStamp: DateTime.Now
            );
        }

        private MessageRecord LogIn(MessageRecord request, Socket userSocket)
        {
            UserAuthDataDTO userAuth = UserAuthDataDTO.CreateFromString(request.message);
            using DbGateway db = new();
            User? user = db.GetUser(request.guidSender);
            DateTime dateTime = DateTime.Now;

            //check user data
            string error = string.Empty;
            if (user is null)
                error = "User not found.";
            else if (user.Login != userAuth.login || _hasher.Verify(userAuth.pass, user.HashedPassword) == false)
                error = "Login or password are incorrect.";
            if (string.IsNullOrEmpty(error) == false)
                return new(
                    dateTime,
                    Guid,
                    null,
                    Enum_CommandType.INFO_FROM_SERVER | Enum_CommandType.LOG_IN,
                    error);
            //success authentication
            if (string.Equals(userAuth.name, user!.Name))
                user.Name = userAuth.name;
            user.IsOnline = true;
            user.LastLogin = dateTime;
            db.Update(user);
            db.SaveChanges();
            if (_guidSockets.ContainsKey(user.Guid) == false)
                _guidSockets.Add(user.Guid, userSocket);
            _logger.Writeline($"{dateTime} {request.guidSender}: {userAuth.name} logged in chat in");
            return new(
                dateTime,
                Guid,
                null,
                Enum_CommandType.CLIENT_INVITE | Enum_CommandType.INFO_FROM_SERVER,
                $"{userAuth.name}{(char)29}{request.guidSender}");
        }

        private MessageRecord SendTo(MessageRecord request) => request;
        #endregion

        private void RemoveConnection()
        {
            using DbGateway db = new();
            List<User> users = new();
            foreach (var guidSocket in _guidSockets)
            {
                User? user = db.GetUser(guidSocket.Key);
                if (user is not null)
                    users.Add(user);
                guidSocket.Value.Shutdown(SocketShutdown.Both);
                guidSocket.Value.Close();
            }
            db.UpdateUserRangeOnlineData(isOnline: false, DateTime.Now);
            Dispose();
        }

        public override async Task SendAsync(MessageRecord message, Socket? clientSocket = null)
        {
            Socket s = clientSocket ?? _socketBase!;
            if (message.receivers is not null)
            {
                using DbGateway db = new();
                try
                {
                    await db.BeginTransactionAsync();
                    MessageModel mesMod = new()
                    {
                        Data = message.message,
                        SenderGuid = message.guidSender
                    };
                    db.Add(mesMod);
                    db.SaveChanges();
                    foreach (Guid receiver in message.receivers!)
                    {
                        db.Add(new UserMessageReceivers()
                        {
                            MessageModelId = mesMod.Id,
                            UserReceiveGuid = receiver
                        });
                    }
                    db.SaveChanges();
                    await db.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    await db.RollBacktransactionAsync();
                    _logger.WritelineError($"{ex.Message}\nInner: {ex.InnerException?.Message}");
                    return;
                }
            }
            await s.SendToAsync(
                message.GetBytesJson(_encoding),
                SocketFlags.None,
                s.RemoteEndPoint!,
                _token);
        }
    }
}