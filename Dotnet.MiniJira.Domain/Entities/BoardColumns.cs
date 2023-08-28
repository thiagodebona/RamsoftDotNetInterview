using Dotnet.MiniJira.Domain.Core;
using Dotnet.MiniJira.Domain.Enums.Board;

namespace Dotnet.MiniJira.Domain.Entities;

public class BoardColumns : Entity
{
    public ColumnType Type { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateCreate { get; set; } = DateTime.Now;
    public List<Task>? Tasks { get; set; }
}
