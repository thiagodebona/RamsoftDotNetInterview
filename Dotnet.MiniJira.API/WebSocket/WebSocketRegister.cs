using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Models.Broadcaster;
using Dotnet.MiniJira.Domain.Models.Users;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Dotnet.MiniJira.API.Notifications;

public interface IWebSocketRegister
{
    public void SetUpLiveNotificationsServer(WebApplication app, string runningIp);
    public Task Broadcast(string message);
}

public class WebSocketRegister : IWebSocketRegister
{
    private IUserService _userService;
    private ILogger<WebSocketRegister> _logger;

    public List<Tuple<WebSocket, AuthenticateResponse>> connections = new List<Tuple<WebSocket, AuthenticateResponse>>();

    public WebSocketRegister(IUserService userService, ILogger<WebSocketRegister> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public void SetUpLiveNotificationsServer(WebApplication app, string runningIp)
    {
        try
        {
            _logger.LogInformation($"------> Live data streamming running Web socket server on '{runningIp}'");
            app.Map("/ws", async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var username = context.Request.Query["username"];
                    var password = context.Request.Query["password"];
                    var login = await _userService.Authenticate(new AuthenticateRequest
                    {
                        Username = username,
                        Password = password
                    }, $"{username}-Client-Remote");

                    using var ws = await context.WebSockets.AcceptWebSocketAsync();

                    var userInfo = new Tuple<WebSocket, AuthenticateResponse>(ws, login);
                    connections.Add(userInfo);

                    await Broadcast(JsonSerializer.Serialize(new BroadCasterMessageModel {
                         Data = new List<dynamic>(),
                         Message = $"Hello {userInfo.Item2.Name}! Welcome to MiniJira App"
                    }));

                    await ReceiveMessage(ws, async (result, buffer) =>
                    {
                        if (result.MessageType == WebSocketMessageType.Close || ws.State == WebSocketState.Aborted)
                        {
                            connections.Remove(userInfo);
                            await ws.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                        }
                    });
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            });

            app.MapPost("/ws/notify", async context =>
            {
                context.Request.EnableBuffering();

                var bodyString = "";
                var strReader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 2048, true);
                using (StreamReader reader = strReader)
                {
                    bodyString = await reader.ReadToEndAsync();
                }

                await Broadcast(bodyString);

                context.Response.StatusCode = (int)HttpStatusCode.OK;
            });
        }
        catch (Exception e)
        {
            _logger.LogCritical($"Web socket server running on 'ws' path ");

            Console.WriteLine($"Error at the broadcaster: {e.Message}");
        }
    }

    public async Task ReceiveMessage(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
    {
        var buffer = new byte[1024 * 24];
        while (socket.State == WebSocketState.Open)
            handleMessage(await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None), buffer);
    }

    public async Task Broadcast(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        foreach (var socket in connections.Select(p => p.Item1).ToList())
        {
            if (socket.State == WebSocketState.Open)
                await socket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
