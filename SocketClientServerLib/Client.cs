

using SocketClientServerLib.DTOs;
using SocketClientServerLib.Extensions;
using SocketClientServerLib.Helpers;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClientServerLib
{
   
    public sealed class Client : SocketBase
    {
        public bool IsConnected => _socketBase?.Connected ?? false;
        public IList<UserDTO>? UserList { get; private set; }
        public Guid? ServerGuid { get; private set; }
        public event EventHandler<MessageRecevedEventArgs> MessageReceived;

        public Client(
            Encoding encoding,
            Logger logger,
            CancellationToken token) : base(encoding, logger, token)
        {
            UserList = new List<UserDTO>();
        }

        public async Task ConnectAsync(EndPoint ePoint)
        {
            if (_socketBase is null)
                InitSocket();
            try
            {
                await _socketBase!.ConnectAsync(ePoint, _token);
                string message = _socketBase.Connected ? "Соединение установлено" : "Нет соединения";
                _logger.Writeline(message);
                MessageRecord responseServer = await ReceiveDataAsync(_socketBase);
                _logger.Writeline($"{responseServer.message} [guid сервера]");
                if (Guid.TryParse(responseServer.message, out Guid guidServer))
                {
                    UserList!.Add(new(guidServer, "сервер", true));
                    ServerGuid = guidServer;
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.WritelineError(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.WritelineError(ex.Message);
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
    }
}
