using Application.Services;
using ApiDoces.Helpers;
using ApiDoces.Responses;
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
        if (user == null)
            return BadRequest(ApiResponse.BadRequest(ApiMessages.InvalidData));

        await userService.CreateUser(user);

        return Ok(ApiResponse.Success(ApiMessages.UserCreated));
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var users = await userService.GetAllUsers();

        return Ok(ApiResponse.Success(users));
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await userService.GetById(id);

        if (user == null)
            return NotFound(ApiResponse.NotFound(ApiMessages.UserNotFound));

        return Ok(ApiResponse.Success(user));
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(int id, InputUserDto user)
    {
        var updatedUser = await userService.UpdateUser(id, user);

        if (updatedUser == null)
            return NotFound(ApiResponse.NotFound(ApiMessages.UserNotFound));

        return Ok(ApiResponse.Success(updatedUser, ApiMessages.UserUpdated));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var deleted = await userService.DeleteUser(id);

        if (!deleted)
            return NotFound(ApiResponse.NotFound(ApiMessages.UserNotFound));

        return Ok(ApiResponse.Success(ApiMessages.UserDeleted));
    }
}