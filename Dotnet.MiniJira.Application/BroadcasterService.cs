namespace Dotnet.MiniJira.Application;

using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Helpers;
using Dotnet.MiniJira.Domain.Models.Broadcaster;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

public class BroadcasterService : IBroadcasterService
{
    private readonly ILogger<BroadcasterService> _logger;
    private readonly AppSettings _appSettings;

    public BroadcasterService(IOptions<AppSettings> appSettings, ILogger<BroadcasterService> logger)
    {
        _logger = logger;
        _appSettings = appSettings.Value;
    }

    public async Task<bool> BroadcastEvent(BroadCasterMessageModel message)
    {
        await new HttpClient().PostAsync($"{_appSettings.ServerUrl}/ws/notify", new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json"), new CancellationToken());

        _logger.LogInformation($"    -> Message brodcasted: {message}");

        return true;
    }
}
