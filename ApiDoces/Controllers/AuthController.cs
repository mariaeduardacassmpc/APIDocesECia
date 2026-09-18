using Application.Helpers;
using ApiDoces.Responses;
using Application.Dtos.Auth;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiDoces.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await authService.Login(dto);

        return Ok(ApiResponses.Success(result));
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        await authService.ForgotPassword(dto);

        return Ok(ApiResponses.Success(ApiMessages.Auth.PasswordResetRequested));
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(PasswordResetDto dto)
    {
        await authService.ResetPassword(dto);

        return Ok(ApiResponses.Success(ApiMessages.Auth.PasswordResetSuccess));
    }
}