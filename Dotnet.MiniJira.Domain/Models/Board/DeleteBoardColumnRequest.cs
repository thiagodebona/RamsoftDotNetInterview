using Dotnet.MiniJira.Domain.Core;

namespace Dotnet.MiniJira.Domain.Models.Board
{
    public class DeleteBoardColumnRequest
    {
        public string BoardId { get; set; } = string.Empty;
        public string ColumnId { get; set; } = string.Empty;
    }
}
