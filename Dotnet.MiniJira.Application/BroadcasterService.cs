namespace Dotnet.MiniJira.Application;

using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

public class BroadcasterService : IBroadcasterService
{
    private readonly ILogger<BroadcasterService> _logger;
    private readonly AppSettings _appSettings;

    public BroadcasterService(IOptions<AppSettings> appSettings, ILogger<BroadcasterService> logger)
    {
        _logger = logger;
        _appSettings = appSettings.Value;
    }

    public async Task<bool> BroadcastEvent(string message)
    {
        await new HttpClient().PostAsync($"{_appSettings.ServerUrl}/ws/notify", new StringContent(message, Encoding.UTF8, "application/json"));

        _logger.LogInformation($"    -> Message brodcasted: {message}");

        return true;
    }
}
