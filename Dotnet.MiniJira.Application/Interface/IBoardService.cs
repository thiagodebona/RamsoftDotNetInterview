namespace Dotnet.MiniJira.Application.Interface;

using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Board;

public interface IBoardService
{
    public Task<Board> CreateBoard(string userId, CreateBoardRequest create);
    public Task<bool> DeleteBoard(string boardId);
    public Task<Board> GetById(string id);
    public Task<List<Board>> GetAll();
    public Task<Board> CreateColum(CreateBoardColumnRequest model);
    public Task<Board> DeleteColum(DeleteBoardColumnRequest model);
}
