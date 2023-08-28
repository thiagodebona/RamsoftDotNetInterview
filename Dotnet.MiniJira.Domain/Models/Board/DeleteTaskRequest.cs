namespace Dotnet.MiniJira.Domain.Models.Board;

public class DeleteTaskRequest
{
    public string BoardId { get; set; } = string.Empty;
    public string TaskId { get; set; } = string.Empty;
}
