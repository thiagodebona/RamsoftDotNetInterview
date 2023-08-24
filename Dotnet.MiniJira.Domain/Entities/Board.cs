namespace Dotnet.MiniJira.Domain.Entities;

using Dotnet.MiniJira.Domain.Core;
using Dotnet.MiniJira.Domain.Models.Board;

public class Board : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string UserCreated { get; set; }
    public List<BoardColumns> Columns { get; set; }
    public List<Task> ArchivedTasks { get; set; }
}