
using DataStorageLayer;
using DataStorageLayer.Models;
using SocketClientServerLib.DTOs;
using SocketClientServerLib.Extensions;
using SocketClientServerLib.Helpers;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace SocketClientServerLib
{
    public abstract class SocketBase: IDisposable
    {
        public Guid Guid { get; set; }
        protected CancellationToken _token;
        protected Logger _logger;
        protected readonly Encoding _encoding;
        protected Socket? _socketBase;

        public SocketBase(Encoding encoding, Logger logger, CancellationToken token)
        {
            InitSocket();
            _token = token;
            _logger = logger;
            _encoding = encoding;
        }
        public abstract Task SendAsync(MessageRecord message, Socket? clientSocket = null);

        protected void InitSocket() => _socketBase = new(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);


        protected async Task<MessageRecord> ReceiveDataAsync(Socket clientSocket)
        {
            StringBuilder data = new();
            byte[] buffer = new byte[1024];
            int sizeReceived;

            do
            {
                sizeReceived = await clientSocket.ReceiveAsync(buffer, SocketFlags.None, _token);
                data.Append(_encoding.GetString(buffer, 0, sizeReceived));
            } while (clientSocket.Available > 0);
            MessageRecord record = JsonSerializer.Deserialize<MessageRecord>(data.ToString())!;
            return record;
        }
        public void Dispose()
        {
            _socketBase?.Dispose();
            _socketBase = null;
        }
    }
}
