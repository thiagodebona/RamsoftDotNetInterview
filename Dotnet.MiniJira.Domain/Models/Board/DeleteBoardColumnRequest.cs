using Dotnet.MiniJira.Domain.Core;

namespace Dotnet.MiniJira.Domain.Models.Board
{
    public class DeleteBoardColumnRequest
    {
        public string boardId { get; set; }
        public string columnId { get; set; }
    }
}
