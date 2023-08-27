using Dotnet.MiniJira.Domain.Models.Broadcaster;

namespace Dotnet.MiniJira.Application.Interface;
public interface IBroadcasterService
{
    public Task<bool> BroadcastEvent(BroadCasterMessageModel message);
}
