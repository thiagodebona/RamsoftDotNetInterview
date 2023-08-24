using Dotnet.MiniJira.Domain.Core;

namespace Dotnet.MiniJira.Domain.Models.Board
{
    public class CreateBoardRequest : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
