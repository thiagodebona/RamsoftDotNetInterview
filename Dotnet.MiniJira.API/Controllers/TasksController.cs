
using Dotnet.MiniJira.API.Authorization;
using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Board;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet.MiniJira.API.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost()]
    public async Task<IActionResult> CreateTask(CreateTaskRequest model)
    {
        return Ok(await _taskService.CreateTask(((User)Request.HttpContext.Items["User"]).Id, model));
    }

    [HttpDelete()]
    public async Task<IActionResult> DeleteTask(DeleteTaskRequest model)
    {
        return Ok(await _taskService.DeleteTask(model));
    }

    [HttpPut()]
    public async Task<IActionResult> UpdateTask(UpdateTaskRequest model)
    {
        return Ok(await _taskService.UpdateTask(model));
    }

    [HttpPost("MoveTask")]
    public async Task<IActionResult> MoveTask(MoveTaskRequest model)
    {
        return Ok(await _taskService.MoveTask(model));
    }

    [HttpPost("Assign")]
    public async Task<IActionResult> Assign(AssignTaskRequest model)
    {
        return Ok(await _taskService.Assign(model));
    }

    [HttpPost("Attachments")]
    public async Task<IActionResult> Attachments(AddAttachmentRequest model)
    {
        return Ok(await _taskService.AddAttachments(model));
    }

    [HttpGet("{boardId}/{taskId}")]
    public async Task<IActionResult> GetById(string boardId, string taskId)
    {
        return Ok(await _taskService.GetById(boardId, taskId));
    }

    [HttpGet("{boardId}")]
    public async Task<IActionResult> GetAll(string boardId)
    {
        return Ok(await _taskService.GetAll(boardId));
    }
}