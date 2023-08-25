namespace WebApi.Controllers;

using Dotnet.MiniJira.API.Authorization;
using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Board;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    private ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost()]
    public async Task<IActionResult> CreateTask(CreateTaskRequest model)
    {
        var userId = (User)Request.HttpContext.Items["User"];
        if (userId != null && userId.Profile != Dotnet.MiniJira.Domain.Enums.User.UserProfile.ADMIN)
        {
            throw new Exception("Only admin profiles can create new boards");
        }

        var board = await _taskService.CreateTask(userId.Id, model);

        return Ok(board);
    }

    [HttpDelete()]
    public async Task<IActionResult> DeleteTask(DeleteTaskRequest model)
    {
        var userId = (User)Request.HttpContext.Items["User"];

        var board = await _taskService.DeleteTask(userId.Id, model);

        return Ok(board);
    }


    [HttpPost("Assign")]
    public async Task<IActionResult> Assign(AssignTaskRequest model)
    {
        var task = await _taskService.Assign(model);
        return Ok(task);
    }

    [HttpPost("Attachments")]
    public async Task<IActionResult> Attachments(AddAttachmentRequest model)
    {
        var task = await _taskService.AddAttachments(model);
        return Ok(task);
    }


    [HttpGet("{boardId}/{taskId}")]
    public async Task<IActionResult> GetById(string boardId, string taskId)
    {
        var board = await _taskService.GetById(boardId, taskId);
        return Ok(board);
    }

    [HttpGet("{boardId}")]
    public async Task<IActionResult> GetAll(string boardId)
    {
        var boards = await _taskService.GetAll(boardId);

        return Ok(boards);
    }
}