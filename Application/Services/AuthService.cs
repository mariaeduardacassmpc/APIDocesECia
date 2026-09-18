using Application.Dtos.Auth;
using Application.Interfaces;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace Application.Services;

public class AuthService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, ITokenService tokenService, ILogger<AuthService> logger, IMemoryCache memoryCache, IEmailService emailService)
{
    public async Task<AuthResponseDto> Login(LoginDto dto)
    {
        logger.LogInformation("Tentativa de login para o e-mail: {Email}", dto.Email);

        var user = await context.User.SingleOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
        {
            logger.LogWarning("Login falhou. Usuário não encontrado para o e-mail: {Email}", dto.Email);
            throw new InvalidOperationException("E-mail ou senha inválidos.");
        }

        var result = passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            logger.LogWarning("Login falhou. Senha inválida para o e-mail: {Email}", dto.Email);
            throw new InvalidOperationException("E-mail ou senha inválidos.");
        }

        var (token, expiresAt) = tokenService.GenerateToken(user);

        logger.LogInformation("Login realizado com sucesso. UserId: {UserId}", user.UserId);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = new UserResponseDto
            {
                Id = user.UserId,
                Email = user.Email
            }
        };
    }

    public async Task ForgotPassword(ForgotPasswordDto dto)
    {
        logger.LogInformation("Solicitação de redefinição de senha para o e-mail: {Email}", dto.Email);

        var user = await context.User
            .SingleOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
        {
            logger.LogWarning("Redefinição de senha falhou. Usuário não encontrado para o e-mail: {Email}", dto.Email);
            throw new InvalidOperationException($"Usuário com e-mail {dto.Email} não encontrado.");
        }

        var token = Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(32)
        )
        .Replace("+", "-")
        .Replace("/", "_")
        .Replace("=", "");

        memoryCache.Set(
            $"password-reset:{dto.Email}",
            token,
            TimeSpan.FromMinutes(30)
        );

        await emailService.SendPasswordResetEmail(
            user.Email,
            token
        );

        logger.LogInformation("E-mail de redefinição enviado com sucesso para: {Email}", dto.Email);
    }

    public async Task ResetPassword(PasswordResetDto dto)
    {
        logger.LogInformation("Tentativa de redefinição de senha para o e-mail: {Email}", dto.Email);

        var cacheKey = $"password-reset:{dto.Email}";

        if (!memoryCache.TryGetValue(cacheKey, out string? storedToken) || storedToken != dto.Token)
        {
            logger.LogWarning("Token de redefinição inválido ou expirado para o e-mail: {Email}", dto.Email);
            throw new InvalidOperationException("Token de redefinição de senha inválido ou expirado.");
        }

        var user = await context.User
            .SingleOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
        {
            logger.LogWarning("Usuário não encontrado para redefinição de senha. E-mail: {Email}", dto.Email);
            throw new InvalidOperationException($"Usuário com e-mail {dto.Email} não encontrado.");
        }

        user.Password = passwordHasher.HashPassword(user, dto.NewPassword);

        await context.SaveChangesAsync();
        memoryCache.Remove(cacheKey);

        logger.LogInformation("Senha alterada com sucesso para o e-mail: {Email}", dto.Email);
    }
}