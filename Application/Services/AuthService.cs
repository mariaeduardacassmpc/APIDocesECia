using Application.Dtos.Auth;
using Application.Interfaces;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace Application.Services;

public class AuthService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, ITokenService tokenService, ILogger<AuthService> logger)
{
    public async Task<AuthResponseDto?> Login(LoginDto dto)
    {
        logger.LogInformation("Tentativa de login para o e-mail: {Email}", dto.Email);

        var user = await context.User.SingleOrDefaultAsync(u => u.Email == dto.Email);

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
}

//    public async Task ForgotPassword(ForgotPasswordDto dto)
//    {
//        var user = await context.User
//            .SingleOrDefaultAsync(u => u.Email == dto.Email);

//        if (user == null)
//            return;

//        var token = Convert.ToBase64String(
//            RandomNumberGenerator.GetBytes(32));

//        var passwordResetToken = new PasswordResetDto
//        {
//            UserId = user.UserId,
//            Token = token,
//            ExpiresAt = DateTime.UtcNow.AddMinutes(30),
//            Used = false
//        };

//        context.PasswordResetToken.Add(passwordResetToken);

//        await context.SaveChangesAsync();

//        // Aqui entraria o envio do e-mail
//        // await emailService.SendPasswordResetEmail(user.Email, token);
//    }

//    public async Task<bool> ResetPassword(PasswordResetDto dto)
//    {
//        var resetToken = await context.PasswordResetDto
//            .Include(x => x.User)
//            .SingleOrDefaultAsync(x =>
//                x.Token == dto.Token &&
//                x.User.Email == dto.Email &&
//                !x.Used);

//        if (resetToken == null)
//            return false;

//        if (resetToken.ExpiresAt < DateTime.UtcNow)
//            return false;

//        resetToken.User.Password = passwordHasher.HashPassword(
//            resetToken.User,
//            dto.NewPassword);

//        resetToken.Used = true;

//        await context.SaveChangesAsync();

//        return true;
//    }
//}