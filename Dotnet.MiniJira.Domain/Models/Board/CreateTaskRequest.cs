namespace Dotnet.MiniJira.Domain.Models.Board
{
    public class CreateTaskRequest
    {
        public string BoardId { get; set; } = string.Empty;
        public string? ColumnId { get; set; } = null;
        public CreateTaskItem Task { get; set; }
    }

    public class CreateTaskItem
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsFavorite { get; set; }
        public DateTime DeadLine { get; set; }
    }
}
