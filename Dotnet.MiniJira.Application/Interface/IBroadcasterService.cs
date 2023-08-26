namespace Dotnet.MiniJira.Application.Interface;
public interface IBroadcasterService
{
    public Task<bool> BroadcastEvent(string message);
}
