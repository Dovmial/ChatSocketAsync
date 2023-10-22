
using SocketClientServerLib;
using SocketClientServerLib.Helpers;
using System.Text;

#region check DB
//if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "SocketChat.db3")) == false)
//{

//}
{
    using DataStorageLayer.DbGateway gw = new(doMigrate: true);
}
#endregion

#region Options
int port = 22222;
int poolSizeClients = 10;
Encoding encoding = Encoding.Unicode;
Logger logger = new(
    Console.Write,
    Console.WriteLine,
    (error) =>
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(error);
        Console.ForegroundColor = ConsoleColor.White;
    });

CancellationTokenSource cts = new();
#endregion

using Listener server = new(
    poolSizeClients,
    encoding,
    logger,
    cts.Token);

await server.StartAsync(port);

