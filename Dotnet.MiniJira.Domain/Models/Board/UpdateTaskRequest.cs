namespace Dotnet.MiniJira.Domain.Models.Board
{
    public class UpdateTaskRequest
    {
        public string BoardId { get; set; } = string.Empty;
        public string TaskId { get; set; } = string.Empty;
        public UpdateTaskItem Task { get; set; }
    }

    public class UpdateTaskItem
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool? IsFavorite { get; set; }
        public DateTime? DeadLine { get; set; }
    }
}
