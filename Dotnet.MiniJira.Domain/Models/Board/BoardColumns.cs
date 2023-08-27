using Dotnet.MiniJira.Domain.Core;
using Dotnet.MiniJira.Domain.Enums.Board;

namespace Dotnet.MiniJira.Domain.Models.Board
{
    public class BoardColumns : Entity
    {
        public ColumnType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public List<Entities.Task>? Tasks { get; set; }
    }
}
