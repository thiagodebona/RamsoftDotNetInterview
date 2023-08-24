using Dotnet.MiniJira.Domain.Core;

namespace Dotnet.MiniJira.Domain.Models.Board
{
    public class CreateBoardColumnRequest
    {
        public string boardId { get; set; }
        public BoardColumns column { get; set; }
    }
}
