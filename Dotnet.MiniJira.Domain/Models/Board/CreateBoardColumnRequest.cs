using Dotnet.MiniJira.Domain.Core;

namespace Dotnet.MiniJira.Domain.Models.Board
{
    public class CreateBoardColumnRequest
    {
        public string BoardId { get; set; } = string.Empty;
        public BoardColumns Column { get; set; }
    }
}
