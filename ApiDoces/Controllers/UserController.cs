using Application.Helpers;
using ApiDoces.Responses;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserService userService) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(InputUserDto user)
    {
        await userService.CreateUser(user);

        return Ok(ApiResponses.Created<object?>(null, ApiMessages.Created("Usuário")));
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var users = await userService.GetAllUsers();

        return Ok(ApiResponses.Success(users));
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await userService.GetById(id);

        return Ok(ApiResponses.Success(user));
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(int id, InputUserDto user)
    {
        var updatedUser = await userService.UpdateUser(id, user);

        return Ok(ApiResponses.Success(updatedUser, ApiMessages.Updated("Usuário")));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await userService.DeleteUser(id);

        return Ok(ApiResponses.Success<object?>(null, ApiMessages.Deleted("Usuário")));
    }
}