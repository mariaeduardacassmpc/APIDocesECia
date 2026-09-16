using Application.Dtos.User;
using Application.Services;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiDoces.Tests.Services;

public class UserTests
{
    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static Mock<IPasswordHasher<User>> CreatePasswordHasher()
    {
        var hasher = new Mock<IPasswordHasher<User>>();

        hasher.Setup(h => h.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
            .Returns("hashed-password");

        return hasher;
    }

    private static UserService CreateService(ApplicationDbContext context, Mock<IPasswordHasher<User>> passwordHasher)
    {
        var logger = new Mock<ILogger<UserService>>();
        return new UserService(context, passwordHasher.Object, logger.Object);
    }

    [Fact]
    public async Task CreateUser_ShouldCreateUser()
    {
        await using var context = CreateContext();
        var passwordHasher = CreatePasswordHasher();
        var service = CreateService(context, passwordHasher);

        var dto = new InputUserDto
        {
            Email = "maria@email.com",
            Password = "123456"
        };

        var result = await service.CreateUser(dto);

        Assert.NotNull(result);
        Assert.Equal("maria@email.com", result.Email);

        var user = await context.User.FirstOrDefaultAsync(u => u.Email == "maria@email.com");

        Assert.NotNull(user);
        Assert.Equal("maria@email.com", user.Email);
        Assert.Equal("hashed-password", user.Password);

        passwordHasher.Verify(
             h => h.HashPassword(
                 It.IsAny<User>(),
                 It.IsAny<string>()),
             Times.Never);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnAllUsers()
    {
        await using var context = CreateContext();

        context.User.AddRange(
            new User
            {
                UserId = 1,
                Email = "maria@email.com",
                Password = "password-1"
            },
            new User
            {
                UserId = 2,
                Email = "teste@email.com",
                Password = "password-2"
            }
        );

        await context.SaveChangesAsync();

        var passwordHasher = CreatePasswordHasher();
        var service = CreateService(context, passwordHasher);
        var result = (await service.GetAllUsers()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.Email == "maria@email.com");
        Assert.Contains(result, u => u.Email == "teste@email.com");
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnEmptyList_WhenThereAreNoUsers()
    {
        await using var context = CreateContext();

        var passwordHasher = CreatePasswordHasher();
        var service = CreateService(context, passwordHasher);
        var result = (await service.GetAllUsers()).ToList();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetById_ShouldReturnUser_WhenUserExists()
    {
        await using var context = CreateContext();

        context.User.Add(
            new User
            {
                UserId = 1,
                Email = "maria@email.com",
                Password = "hashed-password"
            }
        );

        await context.SaveChangesAsync();

        var passwordHasher = CreatePasswordHasher();
        var service = CreateService(context, passwordHasher);

        var result = await service.GetById(1);

        Assert.NotNull(result);
        Assert.Equal("maria@email.com", result.Email);
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenUserDoesNotExist()
    {
        await using var context = CreateContext();

        var passwordHasher = CreatePasswordHasher();
        var service = CreateService(context, passwordHasher);

        var result = await service.GetById(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateUser_ShouldUpdateEmailAndPassword_WhenUserExists()
    {
        await using var context = CreateContext();

        context.User.Add(
            new User
            {
                UserId = 1,
                Email = "old@email.com",
                Password = "old-hashed-password"
            }
        );

        await context.SaveChangesAsync();

        var passwordHasher = CreatePasswordHasher();
        var service = CreateService(context, passwordHasher);

        var dto = new InputUserDto
        {
            Email = "new@email.com",
            Password = "new-password"
        };

        var result = await service.UpdateUser(1, dto);

        Assert.NotNull(result);
        Assert.Equal("new@email.com", result.Email);

        var user = await context.User.FindAsync(1);

        Assert.NotNull(user);
        Assert.Equal("new@email.com", user.Email);
        Assert.Equal("hashed-password", user.Password);

        passwordHasher.Verify(
            h => h.HashPassword(
                It.IsAny<User>(),
                "new-password"),
            Times.Once);
    }
 
    [Fact]
    public async Task UpdateUser_ShouldReturnNull_WhenUserDoesNotExist()
    {
        await using var context = CreateContext();

        var passwordHasher = CreatePasswordHasher();
        var service = CreateService(context, passwordHasher);

        var dto = new InputUserDto
        {
            Email = "new@email.com",
            Password = "new-password"
        };

        var result = await service.UpdateUser(999, dto);
 
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteUser_ShouldDeleteUser_WhenUserExists()
    {
        await using var context = CreateContext();

        context.User.Add(
            new User
            {
                UserId = 1,
                Email = "maria@email.com",
                Password = "hashed-password"
            }
        );

        await context.SaveChangesAsync();

        var passwordHasher = CreatePasswordHasher();
        var service = CreateService(context, passwordHasher);
        var result = await service.DeleteUser(1);

        Assert.True(result);

        var user = await context.User.FindAsync(1);

        Assert.Null(user);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        await using var context = CreateContext();

        var passwordHasher = CreatePasswordHasher();
        var service = CreateService(context, passwordHasher);

        var result = await service.DeleteUser(999);

        Assert.False(result);
    }
}