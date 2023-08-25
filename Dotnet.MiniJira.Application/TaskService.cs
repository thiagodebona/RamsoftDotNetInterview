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

        foreach (var column in board.Columns.Where(cl => cl.Tasks?.Any() != null))
        {
            if (column.Tasks == null || !column.Tasks.Any())
                continue;

            foreach (var task in column.Tasks)
            {
                if (task.Id == model.TaskId)
                {
                    if (task.Attachments == null)
                        task.Attachments = new List<string>();

                    task.Attachments.AddRange(model.Attachments);

                    await _boardRepository.UpdateAsync(board, new CancellationToken());

                    return task;
                }
            }
        }

        throw new Exception("Something went wrong while trying to insert attachments to this task");
    }

    public async Task<Task> Assign(AssignTaskRequest model)
    {
        if (string.IsNullOrEmpty(model.BoardId) || string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.TaskId))
            throw new Exception("The fields, boardid, userId and taskId are required for this operation");

        var board = await _boardService.GetById(model.BoardId);

        var user = await _userService.GetById(model.UserId);

        foreach (var column in board.Columns.Where(cl => cl.Tasks?.Any() != null))
        {
            if (column.Tasks == null || !column.Tasks.Any())
                continue;

            foreach (var task in column.Tasks)
            {
                if (task.Id == model.TaskId)
                {
                    task.Assignee = user.Id;

                    await _boardRepository.UpdateAsync(board, new CancellationToken());

                    return task;
                }
            }
        }

        throw new Exception("Something went wrong while trying to assigne the task to a user");
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
        if (board == null)
            throw new KeyNotFoundException($"Board {model.BoardId} not found");

        if (board.Columns == null || !board.Columns.Any())
            throw new Exception($"The board {model.BoardId} has no column, to create a new task you need at least one column in your board!");


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

    public async Task<Board> DeleteTask(string userId, DeleteTaskRequest model)
    {
        var board = await _boardService.GetById(model.BoardId);
        if (board == null)
            throw new KeyNotFoundException($"Board {model.BoardId} not found");


        var columnToUse = board.Columns?.FirstOrDefault(p => p.Id == model.ColumnId);
        if (columnToUse == null)
            throw new KeyNotFoundException($"Column {model.ColumnId} not found");

        if (columnToUse.Tasks == null || !columnToUse.Tasks.Any())
            throw new KeyNotFoundException($"The column {model.ColumnId} has no tasks to be deleted");

        var taskToDelete = columnToUse.Tasks.FirstOrDefault(p => p.Id == model.TaskId);
        if (taskToDelete == null)
            throw new KeyNotFoundException($"task {model.TaskId} not found");


        var indexOfColumnToUse = board.Columns.IndexOf(columnToUse);
        var indexOfTask = board.Columns.ElementAt(indexOfColumnToUse).Tasks.IndexOf(taskToDelete);

        board.Columns.ElementAt(indexOfColumnToUse).Tasks.RemoveAt(indexOfTask);

        await _boardRepository.UpdateAsync(board, new CancellationToken());

        return board;
    }

    public async Task<List<Task>> GetAll(string boardId)
    {
        var board = await _boardService.GetById(boardId);
        if (board == null)
            throw new KeyNotFoundException($"Board {boardId} not found");

        var allTasks = new List<Task>();
        board.Columns.ForEach(item => item.Tasks?.ForEach(tsk => allTasks.Add(tsk)));

        return allTasks;
    }

    public async Task<Task> GetById(string boardId, string taskId)
    {
        var board = await _boardService.GetById(boardId);
        if (board == null)
            throw new KeyNotFoundException($"Board {boardId} not found");

        Task taskByid = new Task();
        foreach (var item in board.Columns)
        {
            var found = item.Tasks?.FirstOrDefault(tsk => tsk.Id == taskId);
            if (found != null)
            {
                taskByid = found;
                break;
            }
        }

        if (taskByid == null)
            throw new KeyNotFoundException($"Task {taskId} not found");

        return taskByid;
    }
}
