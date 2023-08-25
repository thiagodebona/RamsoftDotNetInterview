namespace Dotnet.MiniJira.Application.Interface;

using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Board;

public interface ITaskService
{
    public Task<Task> GetById(string boardId, string taskId);
    public Task<List<Task>> GetAll(string boardId);
    public Task<Board> CreateTask(string userId, CreateTaskRequest model);
    public Task<Board> DeleteTask(string userId, DeleteTaskRequest model);
}
