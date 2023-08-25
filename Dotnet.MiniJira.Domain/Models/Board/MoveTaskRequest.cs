namespace Dotnet.MiniJira.Domain.Models.Board;

public class MoveTaskRequest
{
    public string BoardId { get; set; } = string.Empty;
    public string TaskId { get; set; } = string.Empty;
    public string NewColumnId { get; set; } = string.Empty;
}
