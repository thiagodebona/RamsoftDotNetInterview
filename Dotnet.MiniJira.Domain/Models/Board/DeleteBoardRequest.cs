namespace Dotnet.MiniJira.Domain.Models.Board
{
    public class UpdateBoardRequest
    {
        public string BoardId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
