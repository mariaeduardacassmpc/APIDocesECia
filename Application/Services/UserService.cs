using Application.Dtos.User;
using Application.Mappings;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class UserService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
{
    public async Task<UserDto> CreateUser(CreateUserDto dto)
    {
        var user = new User
        {
            Email = dto.Email,
            Password = string.Empty 
        };

        user.Password = passwordHasher.HashPassword(user, dto.Password);

        context.User.Add(user);
        await context.SaveChangesAsync();

        return user.ToDto();
    }

    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        var users = await context.User.ToListAsync();
        return users.Select(u => u.ToDto());
    }

    public async Task<UserDto?> GetById(int id)
    {
        var user = await context.User.FindAsync(id);
        return user?.ToDto();
    }

    public async Task<UserDto?> UpdateUser(int id, UpdateUserDto dto)
    {
        var existingUser = await context.User.FindAsync(id);

        if (existingUser == null)
            return null;

        existingUser.Email = dto.Email;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            existingUser.Password = passwordHasher.HashPassword(existingUser, dto.Password);

        await context.SaveChangesAsync();

        return existingUser.ToDto();
    }

    public async Task<bool> DeleteUser(int id)
    {
        var user = await context.User.FindAsync(id);

        if (user == null)
            return false;

        context.User.Remove(user);
        await context.SaveChangesAsync();

        return true;
    }
}   