namespace WebApi.Controllers;

using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Board;
using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    private IBoardService _boardService;

    public TasksController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateBoard(CreateBoardRequest model)
    {
        var userId = (User)Request.HttpContext.Items["User"];
        if (userId.Profile != Dotnet.MiniJira.Domain.Enums.User.UserProfile.ADMIN)
        {
            throw new Exception("Only admin profiles can create new boards");
        }

        var board = await _boardService.CreateBoard(userId.Id, model);

        return Ok(board);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var board = await _boardService.GetById(id);
        return Ok(board);
    }

    [HttpDelete("DeleteColum")]
    public async Task<IActionResult> DeleteColum(DeleteBoardColumnRequest model)
    {
        var user = await _boardService.DeleteColum(model);
        return Ok(user);
    }

    [HttpPost("CreateColum")]
    public async Task<IActionResult> CreateColum(CreateBoardColumnRequest model)
    {
        var board = await _boardService.CreateColum(model);
        return Ok(board);
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var boards = await _boardService.GetAll();

        return Ok(boards);
    }
}