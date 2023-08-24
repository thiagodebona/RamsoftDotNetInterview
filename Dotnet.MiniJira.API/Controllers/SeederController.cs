﻿namespace WebApi.Controllers;

using Dotnet.MiniJira.Application.Interface;
using Dotnet.MiniJira.Domain.Models.Users;
using Microsoft.AspNetCore.Mvc;
using WebApi.Authorization;

[Authorize]
[ApiController]
[Route("[controller]")]
public class SeederController : ControllerBase
{
    private IUserService _userService;

    public SeederController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest model)
    {
        var response = await _userService.Authenticate(model, "http://localhost");
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser(CreateUserRequest model)
    {
        var user = await _userService.CreateUser(model, "http://localhost");
        var response = await _userService.Authenticate(new AuthenticateRequest
        {
            Password = model.Password,
            Username = model.Username
        }, "http://localhost");

        return Ok(response);
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var user = _userService.GetById(id);
        return Ok(user);
    }
}