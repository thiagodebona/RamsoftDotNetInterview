using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Broadcaster;
using Dotnet.MiniJira.Notification.Client;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

var ws = new ClientWebSocket();

string username, password;
while (true)
{
    Console.WriteLine("Require authentication, please writte you username and then you password");
    Console.Write("Username: ");
    username = Console.ReadLine();
    Console.Write("Password: ");
    password = Console.ReadLine();
    break;
}


Console.WriteLine("Connecting to server");
await ws.ConnectAsync(new Uri($"ws://localhost:7100/ws?username={username}&password={password}"), CancellationToken.None);
Console.WriteLine("Successfuly connected!");

var receiveTask = System.Threading.Tasks.Task.Run(async () =>
{
    var buffer = new byte[1024 * 4];
    while (true)
    {
        var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        if (result.MessageType == WebSocketMessageType.Close)
        {
            break;
        }

        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        var notification = JsonSerializer.Deserialize<BroadCasterMessageModel>(message);
        if (notification.Data == null)
            Console.WriteLine($"{DateTime.Now.ToUniversalTime()} -> {notification.Message}");
        else
        {
            Console.WriteLine($"{DateTime.Now.ToUniversalTime()} -> New board update {notification.Message}");
            Console.WriteLine(new TableCreatorHelper().CreateTable(new List<Board> { notification.Data }));
        }
    }
});


await System.Threading.Tasks.Task.WhenAll(receiveTask);

if (ws.State != WebSocketState.Closed)
{
    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
}