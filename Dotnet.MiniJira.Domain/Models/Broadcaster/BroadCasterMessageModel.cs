
namespace Dotnet.MiniJira.Domain.Models.Broadcaster;
public class BroadCasterMessageModel
{
    public string Message { get; set; }
    public List<Entities.Board> Data { get; set; }
}

