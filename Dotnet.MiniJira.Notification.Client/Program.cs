using System.Net.WebSockets;
using System.Text;

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

var receiveTask = Task.Run(async () =>
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
        Console.WriteLine($"{DateTime.Now.ToUniversalTime()} -> {message}");
    }
});


await Task.WhenAll(receiveTask);

if (ws.State != WebSocketState.Closed)
{
    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
}