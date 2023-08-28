namespace Dotnet.MiniJira.Domain.Models.Board;

using Dotnet.MiniJira.Domain.Core;

public class CreateBoardRequest : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
}
