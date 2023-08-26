namespace WebApi.Controllers;

using Dotnet.MiniJira.API.Authorization;
using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Models.Board;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[controller]")]
public class BoardController : ControllerBase
{
    private IBoardService _boardService;

    public BoardController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    [HttpPost()]
    public async Task<IActionResult> CreateBoard(CreateBoardRequest model)
    {
        var board = await _boardService.CreateBoard((User)Request.HttpContext.Items["User"], model);

        return Ok(board);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBoard(string id)
    {
        await _boardService.DeleteBoard((User)Request.HttpContext.Items["User"], id);

        return Ok();
    }

    [HttpPut()]
    public async Task<IActionResult> UpdateBoard([FromBody] UpdateBoardRequest model)
    {
        var resultUpdate = await _boardService.UpdateBoard((User)Request.HttpContext.Items["User"], model);

        return Ok(resultUpdate);
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
        var board = await _boardService.DeleteColum(model);
        return Ok(board);
    }

    [HttpPost("CreateColum")]
    public async Task<IActionResult> CreateColum(CreateBoardColumnRequest model)
    {
        var board = await _boardService.CreateColum(model);
        return Ok(board);
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll(string sortBy = "Name desc")
    {
        var boards = await _boardService.GetAll(sortBy);

        return Ok(boards);
    }
}