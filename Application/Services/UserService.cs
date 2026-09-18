using Application.Dtos.User;
using Application.Mappings;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class UserService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher, ILogger<UserService> logger)
{
    public async Task<UserDto> CreateUser(InputUserDto dto)
    {
        logger.LogInformation("Criando usuário com e-mail: {Email}", dto.Email);

        var emailExists = await context.User.AnyAsync(u => u.Email == dto.Email);

        if (emailExists)
            throw new InvalidOperationException("Já existe um cadastro com esse e-mail.");

        var user = new User
        {
            Email = dto.Email,
            Password = string.Empty
        };

        user.Password = passwordHasher.HashPassword(user, dto.Password);

        context.User.Add(user);
        await context.SaveChangesAsync();

        logger.LogInformation("Usuário criado com sucesso. Id: {UserId}", user.UserId);

        return user.ToDto();
    }

    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        logger.LogInformation("Buscando todos os usuários");

        var users = await context.User.ToListAsync();

        logger.LogInformation("Foram encontrados {Count} usuários", users.Count);

        return users.Select(u => u.ToDto());
    }

    public async Task<UserDto> GetById(int id)
    {
        logger.LogInformation("Buscando usuário por Id: {UserId}", id);

        var user = await context.User.FindAsync(id);

        if (user == null)
        {
            logger.LogWarning("Usuário não encontrado. Id: {UserId}", id);
            throw new InvalidOperationException($"Usuário com Id {id} não encontrado.");
        }

        return user.ToDto();
    }

    public async Task<UserDto> UpdateUser(int id, InputUserDto dto)
    {
        logger.LogInformation("Atualizando usuário. Id: {UserId}", id);

        var existingUser = await context.User.FindAsync(id);

        if (existingUser == null)
        {
            logger.LogWarning("Usuário não encontrado para atualização. Id: {UserId}", id);
            throw new InvalidOperationException($"Usuário com Id {id} não encontrado.");
        }

        var emailExists = await context.User.AnyAsync(u => u.Email == dto.Email && u.UserId != id);

        if (emailExists)
            throw new InvalidOperationException("Já existe um cadastro com esse e-mail.");

        existingUser.Email = dto.Email;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            existingUser.Password = passwordHasher.HashPassword(existingUser, dto.Password);

        await context.SaveChangesAsync();

        logger.LogInformation("Usuário atualizado com sucesso. Id: {UserId}", id);

        return existingUser.ToDto();
    }

    public async Task DeleteUser(int id)
    {
        logger.LogInformation("Excluindo usuário. Id: {UserId}", id);

        var user = await context.User.FindAsync(id);

        if (user == null)
        {
            logger.LogWarning("Usuário não encontrado para exclusão. Id: {UserId}", id);
            throw new InvalidOperationException($"Usuário com Id {id} não encontrado.");
        }

        context.User.Remove(user);
        await context.SaveChangesAsync();

        logger.LogInformation("Usuário excluído com sucesso. Id: {UserId}", id);
    }
}