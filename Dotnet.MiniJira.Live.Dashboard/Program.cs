using Dotnet.MiniJira.Live.Dashboard.App.Helpers;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

ClientWebSocket ws = new ClientWebSocket();
var consoleWr = new ConsoleWriter();
var logo = @"   
 __  __ _       _        _ _             __   ___  
|  \/  (_)     (_)      | (_)           /_ | / _ \ 
| \  / |_ _ __  _       | |_ _ __ __ _   | || | | |
| |\/| | | '_ \| |  _   | | | '__/ _` |  | || | | |
| |  | | | | | | | | |__| | | | | (_| |  | || |_| |
|_|  |_|_|_| |_|_|  \____/|_|_|  \__,_|  |_(_)___/ 
";
string username, password;
var connectedAt = DateTime.Now;

while (true)
{
    Console.Clear();
    
    consoleWr.WriteLine("{FC=Blue}" + logo + "\n{/FC}");
    consoleWr.WriteLine("{FC=Yellow}Please insert your credentials to procced\n{/FC}");

    Console.Write("Username: ");
    username = Console.ReadLine();

    Console.Write("Password: ");
    password = Console.ReadLine();

    try
    {
        ws = new ClientWebSocket();

        await ws.ConnectAsync(new Uri($"ws://localhost:7100/ws?username={username}&password={password}"), CancellationToken.None);
        Console.Clear();

        connectedAt = DateTime.Now;

        consoleWr.WriteLine("{FC=Blue}" + logo + "\n{/FC}");

        consoleWr.WriteLine("{FC=Green}" + connectedAt.ToString("yyyy/MM/dd H:mm") + " -> Successfuly connected!{/FC}");
        break;
    }
    catch (Exception e)
    {

        consoleWr.WriteLine("{FC=Red}\nUnauthorized, try again..{/FC}");
        consoleWr.WriteLine("{FC=Yellow}Retrying connection{/FC}");

        ws.Dispose();

        Thread.Sleep(500);

        Console.Clear();
    }
}


var receiveTask = Task.Run(async () =>
{
    var buffer = new byte[1024 * 32];
    while (true)
    {
        var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        if (result.MessageType == WebSocketMessageType.Close)
        {
            break;
        }

        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        var notification = JsonSerializer.Deserialize<Dotnet.MiniJira.Domain.Models.Broadcaster.BroadCasterMessageModel>(message);
        if (notification.Data == null)
            consoleWr.WriteLine("{FC=Blue}" + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + " -> " + notification.Message + "\n{/FC}");
        else
        {
            Console.Clear();

            consoleWr.WriteLine("{FC=Blue}" + logo + "\n{/FC}");

            consoleWr.WriteLine("{FC=Green}" + connectedAt.ToString("yyyy/MM/dd HH:mm") + " -> Successfuly connected!{/FC}");

            consoleWr.WriteLine("{FC=Yellow}" + connectedAt.ToString("yyyy/MM/dd HH:mm") + " -> " + notification.Message + "{/FC}");

            consoleWr.Write((new TableCreatorHelper().CreateTable(notification.Data)));

        }
    }
});


await Task.WhenAll(receiveTask);

if (ws.State != WebSocketState.Closed)
{
    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
}