using Application.Dtos.User;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController(UserService UserService) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(CreateUserDto user)
    {
        if (user == null)
            return BadRequest(new { mensagem = "User invalid" });

        await UserService.CreateUser(user);

        return Ok(new
        {
            mensagem = "User created successfully!"
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await UserService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await UserService.GetById(id);

        if (user == null)
            return NotFound(new { mensagem = "User not found" });

        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserDto user)
    {
        var updatedUser = await UserService.UpdateUser(id, user);

        if (updatedUser == null)
            return NotFound(new { mensagem = "User not found" });

        return Ok(new
        {
            mensagem = "User updated successfully",
            user = updatedUser
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var deleted = await UserService.DeleteUser(id);

        if (!deleted)
            return NotFound(new { mensagem = "User not found" });

        return Ok(new
        {
            mensagem = "User successfully deleted!"
        });
    }
}