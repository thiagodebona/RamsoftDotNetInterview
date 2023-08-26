namespace Dotnet.MiniJira.Application;

using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Enums.Board;
using Dotnet.MiniJira.Domain.Helpers;
using Dotnet.MiniJira.Domain.Models.Board;
using Dotnet.MiniJira.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

public class BoardService : IBoardService
{
    private readonly ILogger<UserService> _logger;
    private readonly IMongoBaseRepository<Board> _boardRepository;
    private readonly IBroadcasterService _broadcasterService;

    public BoardService(
        ILogger<UserService> logger,
        IMongoBaseRepository<Board> boardRepository,
        IBroadcasterService broadcasterService)
    {
        _logger = logger;
        _boardRepository = boardRepository;
        _broadcasterService = broadcasterService;
    }

    public async Task<Board> CreateBoard(User user, CreateBoardRequest model)
    {
        if (user.Profile != Domain.Enums.User.UserProfile.ADMIN)
        {
            throw new Exception("Only admin profiles can create new boards");
        }

        var board = (await _boardRepository.FindBy(x => x.Name == model.Name, new CancellationToken())).FirstOrDefault();

        if (board == null)
        {
            var boardToAdd = new Board
            {
                Name = model.Name,
                Description = model.Description,
                UserCreated = user.Id,
                Columns = new List<BoardColumns>
                {
                   new BoardColumns {
                       Name = "Todo",
                       Description = "All the todo tasks",
                       Type = ColumnType.todo,
                       Tasks = new List<Task>()
                   },
                   new BoardColumns {
                       Name = "In progress",
                       Description = "In progress tasks",
                       Type = ColumnType.InProgress,
                       Tasks = new List<Task>()
                   },
                   new BoardColumns {
                       Name = "Testing",
                       Description = "Testing tasks",
                       Type = ColumnType.InTest,
                       Tasks = new List<Task>()
                   },
                   new BoardColumns {
                       Name = "Done",
                       Description = "Done tasks",
                       Type = ColumnType.Done,
                       Tasks = new List<Task>()
                   },
                }
            };

            _logger.LogInformation("A new board has just been created");

            await _boardRepository.AddAsync(boardToAdd, new CancellationToken());

            await _broadcasterService.BroadcastEvent($"A new board has just been created: {boardToAdd.Name}");

            return await _boardRepository.GetByIdAsync(boardToAdd.Id, new CancellationToken());
        }

        throw new AppException("Board with the same name is already registered");
    }

    public async Task<bool> DeleteBoard(User user, string boardId)
    {
        if (user.Profile != Domain.Enums.User.UserProfile.ADMIN)
        {
            throw new Exception("Only admin profiles can create new boards");
        }

        var board = (await _boardRepository.FindBy(x => x.Id == boardId, new CancellationToken())).FirstOrDefault();

        if (board == null) throw new KeyNotFoundException($"Board {boardId} not found");

        await _boardRepository.DeleteAsync(boardId, new CancellationToken());

        await _broadcasterService.BroadcastEvent($"Board '{board.Name}' has just been deleted");

        return true;
    }

    public async Task<Board> UpdateBoard(User user, UpdateBoardRequest model)
    {
        if (user.Profile != Domain.Enums.User.UserProfile.ADMIN)
        {
            throw new Exception("Only admin profiles can create new boards");
        }

        var board = (await _boardRepository.FindBy(x => x.Id == model.BoardId.ToString(), new CancellationToken())).FirstOrDefault();
        if (board == null)
            throw new KeyNotFoundException($"Board {model.BoardId} not found");

        if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Description))
            throw new KeyNotFoundException($"You need to inform at least name or description");

        var oldname = board.Name;
        var oldDesc = board.Description;
        board.Name = !string.IsNullOrEmpty(model.Name) ? model.Name : board.Name;
        board.Description = !string.IsNullOrEmpty(model.Description) ? model.Description : board.Description;

        var message = $"Board name updated from -> '{oldname}', to '{board.Name}', and description from '{oldDesc}', to '{board.Description}'";
        await _broadcasterService.BroadcastEvent(message);

        await _boardRepository.UpdateAsync(board, new CancellationToken());


        return board;
    }

    public async Task<Board> CreateColum(CreateBoardColumnRequest model)
    {
        var board = (await _boardRepository.FindBy(x => x.Id == model.BoardId, new CancellationToken())).FirstOrDefault();
        if (board == null) throw new KeyNotFoundException($"Board {model.BoardId} not found");

        if (board.Columns == null)
        {
            board.Columns = new List<BoardColumns>();
        }

        model.Column.Tasks = new List<Task>();

        board.Columns.Add(model.Column);

        // Update the db record
        await _boardRepository.UpdateAsync(board, new CancellationToken());

        var message = $"New column '{model.Column.Name}' created in the board {board.Name}!";
        await _broadcasterService.BroadcastEvent(message);

        return board;
    }

    public async Task<Board> DeleteColum(DeleteBoardColumnRequest model)
    {
        var board = (await _boardRepository.FindBy(x => x.Id == model.BoardId, new CancellationToken())).FirstOrDefault();
        if (board == null) throw new KeyNotFoundException($"Board {model.BoardId} not found");

        var columnToRemove = board.Columns.FirstOrDefault(p => p.Id == model.ColumnId);
        if (columnToRemove == null) throw new KeyNotFoundException($"Board column {model.ColumnId} not found");

        // Check if it has tasks on it, if yes port them to the archives root field
        if (columnToRemove.Tasks != null && columnToRemove.Tasks.Any())
        {
            if (board.ArchivedTasks == null)
                board.ArchivedTasks = new List<Task>();

            board.ArchivedTasks.AddRange(columnToRemove.Tasks);
        }

        board.Columns.Remove(columnToRemove);

        // Update the db record
        await _boardRepository.UpdateAsync(board, new CancellationToken());

        var message = $"Deleted column '{columnToRemove.Name}' from the board {board.Name}!";
        await _broadcasterService.BroadcastEvent(message);

        return board;
    }

    public async Task<Board> GetById(string id, string sortBy)
    {
        var board = (await _boardRepository.FindBy(x => x.Id == id.ToString(), new CancellationToken())).FirstOrDefault();
        if (board == null) throw new KeyNotFoundException($"Board {id} not found");
        return board;
    }

    public async Task<List<Board>> GetAll(string sortBy)
    {
        var result = await _boardRepository.FindAsync(new CancellationToken());

        if (string.IsNullOrEmpty(sortBy))
            return result;

        var sortConfig = new { Sort = sortBy.Split(' ')[0], Order = sortBy.Split(' ')[1].ToUpper() ?? "ASC" };

        foreach (Board board in result)
            foreach (var column in board.Columns)
                column.Tasks = column.Tasks?.CustomSort(sortConfig.Sort, sortConfig.Order == "DESC");

        return result;
    }
}
