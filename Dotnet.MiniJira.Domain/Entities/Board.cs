namespace Dotnet.MiniJira.Domain.Entities;

using Dotnet.MiniJira.Domain.Core;

public class Board : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateCreate { get; set; } = DateTime.Now;
    public DateTime? DateUpdate { get; set; }
    public User UserCreated { get; set; }
    public List<BoardColumns> Columns { get; set; }
    public List<Task> ArchivedTasks { get; set; }
}