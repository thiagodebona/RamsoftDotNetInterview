namespace Dotnet.MiniJira.Application;

using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Board;
using Dotnet.MiniJira.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

public class TaskService : ITaskService
{
    private readonly ILogger<UserService> _logger;
    private readonly IBoardService _boardService;
    private readonly IUserService _userService;
    private readonly IMongoBaseRepository<Board> _boardRepository;

    public TaskService(
        ILogger<UserService> logger,
        IBoardService boardService,
        IUserService userService,
        IMongoBaseRepository<Board> boardRepository)
    {
        _logger = logger;
        _boardService = boardService;
        _userService = userService;
        _boardRepository = boardRepository;
    }

    public async Task<Task> AddAttachments(AddAttachmentRequest model)
    {
        if (!model.Attachments.Any())
            throw new Exception("Field Attachments is required for this operation");

        var board = await _boardService.GetById(model.BoardId);

        var boardColumn = board.Columns.ElementAt(board.Columns.IndexOf(FindColumnByTask(board, model.TaskId)));

        var item = await FindTaskInBoard(board, model.TaskId);

        var itemToUpdate = boardColumn.Tasks.ElementAt(boardColumn.Tasks.IndexOf(item));
        if (itemToUpdate == null)
            itemToUpdate.Attachments = new List<string>();

        itemToUpdate.Attachments.AddRange(model.Attachments);

        await _boardRepository.UpdateAsync(board, new CancellationToken());

        return item;
    }

    public async Task<Task> Assign(AssignTaskRequest model)
    {
        if (string.IsNullOrEmpty(model.BoardId) || string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.TaskId))
            throw new Exception("The fields, boardid, userId and taskId are required for this operation");

        var board = await _boardService.GetById(model.BoardId);

        var user = await _userService.GetById(model.UserId);

        var boardColumn = board.Columns.ElementAt(board.Columns.IndexOf(FindColumnByTask(board, model.TaskId)));

        var currentColumn = board.Columns.ElementAt(board.Columns.IndexOf(boardColumn));

        var currentTask = currentColumn?.Tasks?.FirstOrDefault(x => x.Id == model.TaskId);

        var indexToUpdate = currentColumn.Tasks?.IndexOf(currentTask);

        var itemToUpdate = board.Columns.ElementAt(board.Columns.IndexOf(boardColumn))?.Tasks?.ElementAt(indexToUpdate.Value);

        if (itemToUpdate.Assignee == user.Id)
            throw new Exception($"This task is already assigned to user {user.Name}");

        itemToUpdate.Assignee = user.Id;

        await _boardRepository.UpdateAsync(board, new CancellationToken());

        return itemToUpdate;
    }

    public async Task<Board> CreateTask(string userId, CreateTaskRequest model)
    {
        if (string.IsNullOrEmpty(model.BoardId))
            throw new KeyNotFoundException($"Board {model.BoardId} not found");

        if (model.Task == null)
            throw new Exception($"You need to inform the Task info to create a task");

        if (string.IsNullOrEmpty(model.Task.Name) ||
            string.IsNullOrEmpty(model.Task.Description))
            throw new Exception($"Task name and description are mandatory");

        var board = await _boardService.GetById(model.BoardId);

        if (board.Columns == null || !board.Columns.Any())
            throw new Exception($"The board {model.BoardId} has no column, to create a new task you need at least one column at it!");


        var firstFoundColumn = board.Columns.FirstOrDefault();
        var columnIdToUse = string.IsNullOrEmpty(model.ColumnId) ? firstFoundColumn?.Id : model.ColumnId;
        var columnToUse = board.Columns.FirstOrDefault(p => p.Id == columnIdToUse);
        if (columnToUse == null)
            throw new Exception("Something went wrong while selecting the column which insert the new task");

        var indexOfColumnToUse = board.Columns.IndexOf(columnToUse);
        var boardColumn = board.Columns.ElementAt(indexOfColumnToUse);
        if (boardColumn.Tasks == null || !boardColumn.Tasks.Any())
            boardColumn.Tasks = new List<Task>();

        var newTask = new Task(model.Task.Name,
                       model.Task.Description,
                       userId, model.Task.DeadLine,
                       model.Task.IsFavorite);

        boardColumn.Tasks.Add(newTask);

        board.Columns[indexOfColumnToUse].Tasks = boardColumn.Tasks;

        await _boardRepository.UpdateAsync(board, new CancellationToken());

        return board;
    }

    public async Task<Task> UpdateTask(UpdateTaskRequest model)
    {
        if (string.IsNullOrEmpty(model.BoardId) || string.IsNullOrEmpty(model.TaskId))
            throw new Exception("The fields, boardid and taskId are required for this operation");

        var board = await _boardService.GetById(model.BoardId);

        var boardColumn = board.Columns.ElementAt(board.Columns.IndexOf(FindColumnByTask(board, model.TaskId)));

        var currentColumn = board.Columns.ElementAt(board.Columns.IndexOf(boardColumn));

        var currentTask = currentColumn?.Tasks?.FirstOrDefault(x => x.Id == model.TaskId);

        var indexToUpdate = currentColumn.Tasks?.IndexOf(currentTask);

        var itemToUpdate = board.Columns.ElementAt(board.Columns.IndexOf(boardColumn))?.Tasks?.ElementAt(indexToUpdate.Value);

        if (string.IsNullOrEmpty(model.Task.Name) &&
            string.IsNullOrEmpty(model.Task.Description) &&
            !model.Task.IsFavorite.HasValue &&
            !model.Task.DeadLine.HasValue)
        {
            throw new Exception("Nothing to update");
        }

        if (!string.IsNullOrEmpty(model.Task.Name) && itemToUpdate.Name != model.Task.Name)
            itemToUpdate.Name = model.Task.Name;

        if (!string.IsNullOrEmpty(model.Task.Description) && itemToUpdate.Description != model.Task.Description)
            itemToUpdate.Description = model.Task.Description;

        if (model.Task.DeadLine.HasValue && itemToUpdate.DeadLine != model.Task.DeadLine.Value)
            itemToUpdate.DeadLine = model.Task.DeadLine.Value;

        if (model.Task.IsFavorite.HasValue && itemToUpdate.IsFavorite != model.Task.IsFavorite)
            itemToUpdate.IsFavorite = model.Task.IsFavorite.Value;

        await _boardRepository.UpdateAsync(board, new CancellationToken());

        return itemToUpdate;
    }

    public async Task<Board> DeleteTask(DeleteTaskRequest model)
    {
        var board = await _boardService.GetById(model.BoardId);

        var boardColumn = board.Columns.ElementAt(board.Columns.IndexOf(FindColumnByTask(board, model.TaskId)));

        var currentColumn = board.Columns.ElementAt(board.Columns.IndexOf(boardColumn));

        if (currentColumn.Tasks == null || !currentColumn.Tasks.Any())
            throw new KeyNotFoundException($"The column {boardColumn.Id} has no tasks to be deleted");

        var currentTask = currentColumn?.Tasks?.FirstOrDefault(x => x.Id == model.TaskId);

        var indexToRemove = currentColumn.Tasks?.IndexOf(currentTask);

        board.Columns.ElementAt(board.Columns.IndexOf(boardColumn))?.Tasks?.RemoveAt(indexToRemove.Value);

        await _boardRepository.UpdateAsync(board, new CancellationToken());

        return board;
    }

    public async Task<List<Task>> GetAll(string boardId)
    {
        var board = await _boardService.GetById(boardId);

        var allTasks = new List<Task>();
        board.Columns.ForEach(item => item.Tasks?.ForEach(tsk => allTasks.Add(tsk)));

        return allTasks;
    }

    public async Task<Task> GetById(string boardId, string taskId)
    {
        var board = await _boardService.GetById(boardId);

        return await FindTaskInBoard(board, taskId);
    }

    public async Task<Board> MoveTask(MoveTaskRequest model)
    {
        if (string.IsNullOrEmpty(model.BoardId) || string.IsNullOrEmpty(model.NewColumnId) || string.IsNullOrEmpty(model.TaskId))
            throw new Exception("The fields, boardid, newColumnId and taskId are required for this operation");

        var board = await _boardService.GetById(model.BoardId);
        var task = await GetById(model.BoardId, model.TaskId);

        // Copy the task to the other new column
        var newTaskColumn = board.Columns.FirstOrDefault(p => p.Id == model.NewColumnId);
        if (newTaskColumn == null)
            throw new Exception($"The new column {model.NewColumnId} could not be found");

        board.Columns.ElementAt(board.Columns.IndexOf(newTaskColumn)).Tasks?.Add(task);

        // Remove it from the previews one
        var oldTaskColumn = FindColumnByTask(board, model.TaskId);
        var taskToRemove = oldTaskColumn.Tasks.FirstOrDefault(p => p.Id == model.TaskId);
        board.Columns.ElementAt(board.Columns.IndexOf(oldTaskColumn)).Tasks?.Remove(taskToRemove);

        await _boardRepository.UpdateAsync(board, new CancellationToken());

        return board;
    }

    private async Task<Task> FindTaskInBoard(Board board, string taskId)
    {
        var itemToReturn = FindColumnByTask(board, taskId);

        var taskToReturn = itemToReturn?.Tasks?.FirstOrDefault(tsk => tsk.Id == taskId);

        if (taskToReturn == null)
            throw new KeyNotFoundException($"Task {taskId} not found");

        return taskToReturn;
    }

    private BoardColumns FindColumnByTask(Board board, string taskId)
    {
        foreach (var item in board.Columns)
        {
            var found = item.Tasks?.FirstOrDefault(tsk => tsk.Id == taskId);
            if (found != null)
            {
                return item;
            }
        }

        throw new KeyNotFoundException($"Task {taskId} not found");
    }
}
