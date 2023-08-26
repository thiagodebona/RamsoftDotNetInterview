namespace Dotnet.MiniJira.Application.Interface;

using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Board;

public interface IBoardService
{
    public Task<Board> CreateBoard(User user, CreateBoardRequest model);
    public Task<bool> DeleteBoard(User user, string boardId);
    public Task<Board> UpdateBoard(User user, UpdateBoardRequest model);
    public Task<Board> GetById(string id, string sortBy = "");
    public Task<List<Board>> GetAll(string sortBy = "");
    public Task<Board> CreateColum(CreateBoardColumnRequest model);
    public Task<Board> DeleteColum(DeleteBoardColumnRequest model);
}
