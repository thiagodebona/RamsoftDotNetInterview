namespace Dotnet.MiniJira.Application.Interface;

using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Board;

public interface ITaskService
{
    public Task<Task> GetById(string boardId, string taskId);
    public Task<List<Task>> GetAll(string boardId);
    public Task<Board> CreateTask(string userId, CreateTaskRequest model);
    public Task<Board> DeleteTask(DeleteTaskRequest model);
    public Task<Task> UpdateTask(UpdateTaskRequest model);
    public Task<Task> Assign(AssignTaskRequest model);
    public Task<Board> MoveTask(MoveTaskRequest model);
    public Task<Task> AddAttachments(AddAttachmentRequest model);
}
