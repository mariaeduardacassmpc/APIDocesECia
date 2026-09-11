using Application.Dtos.Auth;
using Application.Interfaces;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AuthService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, ITokenService tokenService, ILogger<AuthService> logger)
{
    public async Task<AuthResponseDto?> Login(LoginDto dto)
    {
        logger.LogInformation("Tentativa de login para o e-mail: {Email}", dto.Email);

        var user = await context.User.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
        {
            logger.LogWarning("Login falhou. Usuário não encontrado para o e-mail: {Email}", dto.Email);
            return null;
        }

        var result = passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            logger.LogWarning("Login falhou. Senha inválida para o e-mail: {Email}", dto.Email);
            return null;
        }

        var (token, expiresAt) = tokenService.GenerateToken(user);

        logger.LogInformation("Login realizado com sucesso. UserId: {UserId}", user.UserId);

        return new AuthResponseDto { Token = token, ExpiresAt = expiresAt };
    }
}