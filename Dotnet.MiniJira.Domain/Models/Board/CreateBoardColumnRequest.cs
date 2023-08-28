using Dotnet.MiniJira.Domain.Core;
using Dotnet.MiniJira.Domain.Entities;

namespace Dotnet.MiniJira.Domain.Models.Board;

public class CreateBoardColumnRequest
{
    public string BoardId { get; set; } = string.Empty;
    public BoardColumns Column { get; set; }
}
