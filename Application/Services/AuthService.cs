using Application.Dtos.Auth;
using Application.Interfaces;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class AuthService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, ITokenService tokenService)
{
    public async Task<AuthResponseDto?> Login(LoginDto dto)
    {
        var user = await context.User.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            return null;

        var result = passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);

        if (result == PasswordVerificationResult.Failed)
            return null;

        var (token, expiresAt) = tokenService.GenerateToken(user);

        return new AuthResponseDto { Token = token, ExpiresAt = expiresAt };
    }
}