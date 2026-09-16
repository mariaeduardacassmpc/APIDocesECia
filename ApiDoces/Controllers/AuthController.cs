using ApiDoces.Helpers;
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

        if (result == null)
            return Unauthorized(ApiResponse.BadRequest(ApiMessages.InvalidCredentials));

        return Ok(ApiResponse.Success(result));
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        await authService.ForgotPassword(dto);

        return Ok(ApiResponse.Success("Se o e-mail estiver cadastrado, as instruções de recuperação serão enviadas."
        ));
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(PasswordResetDto dto)
    {
        var result = await authService.ResetPassword(dto);

        if (!result)
            return BadRequest(ApiResponse.BadRequest("Token inválido ou expirado."));

        return Ok(ApiResponse.Success("Senha redefinida com sucesso."));
    }
}