using ApiDoces.Services;
using Application.Dtos.Category;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

public class CategoryTests
{
    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static CategoryService CreateService(ApplicationDbContext context)
    {
        var logger = new Mock<ILogger<CategoryService>>();

        return new CategoryService(context, logger.Object);
    }

    [Fact]
    public async Task CreateCategory_ShouldCreateCategory()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        var dto = new CategoryInputDto
        {
            Name = "Doces"
        };

        await service.CreateCategory(dto);

        var category = await context.Category.FirstOrDefaultAsync(c => c.Name == "Doces");

        Assert.NotNull(category);
        Assert.Equal("Doces", category.Name);
    }

    [Fact]
    public async Task GetAllCategories_ShouldReturnAllCategories()
    {
        await using var context = CreateContext();

        context.Category.AddRange(
            new Category { CategoryId = 1, Name = "Doces" },
            new Category { CategoryId = 2, Name = "Salgados" }
        );

        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = (await service.GetAllCategories()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Name == "Doces");
        Assert.Contains(result, c => c.Name == "Salgados");
    }

    [Fact]
    public async Task GetAllCategories_ShouldReturnEmptyList_WhenThereAreNoCategories()
    {
        await using var context = CreateContext();
        
        var service = CreateService(context);
        var result = (await service.GetAllCategories()).ToList();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetById_ShouldReturnCategory_WhenCategoryExists()
    {
        await using var context = CreateContext();

        context.Category.Add(new Category { CategoryId = 1, Name = "Doces" });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.GetById(1);

        Assert.NotNull(result);
        Assert.Equal("Doces", result.Name);
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        await using var context = CreateContext();
        
        var service = CreateService(context);
        var result = await service.GetById(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateCategory_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        await using var context = CreateContext();
        
        var service = CreateService(context);
        var dto = new CategoryInputDto { Name = "Doces" };
        var result = await service.UpdateCategory(999, dto);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteCategory_ShouldDeleteCategory_WhenCategoryExists()
    {
        await using var context = CreateContext();

        context.Category.Add(new Category { CategoryId = 1, Name = "Doces" });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.DeleteCategory(1);

        Assert.True(result);
        Assert.Null(await context.Category.FindAsync(1));
    }

    [Fact]
    public async Task DeleteCategory_ShouldReturnFalse_WhenCategoryDoesNotExist()
    {
        await using var context = CreateContext();
        
        var service = CreateService(context);
        var result = await service.DeleteCategory(999);

        Assert.False(result);
    }
}