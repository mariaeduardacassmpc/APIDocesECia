using ApiDoces.Helpers;
using ApiDoces.Responses;
using Application.Dtos.Auth;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await authService.Login(dto);

        if (result == null)
            return Unauthorized(ApiResponse.BadRequest(ApiMessages.InvalidCredentials));

        return Ok(ApiResponse.Success(result));
    }
}