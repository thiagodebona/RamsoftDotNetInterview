namespace Dotnet.MiniJira.Domain.Models.Board;

public class AssignTaskRequest
{
    public string BoardId { get; set; } = string.Empty;
    public string TaskId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}
