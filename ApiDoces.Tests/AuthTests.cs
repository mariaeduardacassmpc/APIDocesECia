using Application.Dtos.Auth;
using Application.Interfaces;
using Application.Services;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

public class AuthServiceTests
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
        return new Mock<IPasswordHasher<User>>();
    }

    private static Mock<ITokenService> CreateTokenService()
    {
        return new Mock<ITokenService>();
    }

    private static AuthService CreateService(ApplicationDbContext context, Mock<IPasswordHasher<User>> passwordHasher,
        Mock<ITokenService> tokenService)
    {
        var logger = new Mock<ILogger<AuthService>>();
        return new AuthService(context, passwordHasher.Object, tokenService.Object, logger.Object);
    }

    [Fact]
    public async Task Login_ShouldReturnAuthResponse_WhenCredentialsAreValid()
    {
        await using var context = CreateContext();

        var user = new User
        {
            UserId = 1,
            Email = "teste@email.com",
            Password = "hashed-password"
        };

        context.User.Add(user);
        await context.SaveChangesAsync();

        var passwordHasher = CreatePasswordHasher();
        
        passwordHasher.Setup(x => x.VerifyHashedPassword(user, user.Password, "123456"))
            .Returns(PasswordVerificationResult.Success);

        var tokenService = CreateTokenService();
        var expiresAt = DateTime.UtcNow.AddHours(1);
        
        tokenService.Setup(x => x.GenerateToken(user))
            .Returns(("fake-token", expiresAt));

        var service = CreateService(context, passwordHasher, tokenService);

        var dto = new LoginDto
        {
            Email = "teste@email.com",
            Password = "123456"
        };

        var result = await service.Login(dto);

        Assert.NotNull(result);
        Assert.Equal("fake-token", result.Token);
        Assert.Equal(expiresAt, result.ExpiresAt);
        Assert.Equal(1, result.User.Id);
        Assert.Equal("teste@email.com", result.User.Email);
    }

    [Fact]
    public async Task Login_ShouldReturnNull_WhenUserDoesNotExist()
    {
        await using var context = CreateContext();

        var passwordHasher = CreatePasswordHasher();
        var tokenService = CreateTokenService();

        var service = CreateService(context, passwordHasher, tokenService);

        var dto = new LoginDto
        {
            Email = "naoexiste@email.com",
            Password = "123456"
        };

        var result = await service.Login(dto);

        Assert.Null(result);

        tokenService.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Login_ShouldReturnNull_WhenPasswordIsInvalid()
    {
        await using var context = CreateContext();

        var user = new User
        {
            UserId = 1,
            Email = "teste@email.com",
            Password = "hashed-password"
        };

        context.User.Add(user);
        await context.SaveChangesAsync();

        var passwordHasher = CreatePasswordHasher();
        passwordHasher.Setup(x => x.VerifyHashedPassword(user, user.Password, "senha-errada"))
            .Returns(PasswordVerificationResult.Failed);

        var tokenService = CreateTokenService();

        var service = CreateService(context, passwordHasher, tokenService);

        var dto = new LoginDto
        {
            Email = "teste@email.com",
            Password = "senha-errada"
        };

        var result = await service.Login(dto);

        Assert.Null(result);

        tokenService.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Never);
    }
}