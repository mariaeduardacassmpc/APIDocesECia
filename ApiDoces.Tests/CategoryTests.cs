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

        var result = await service.CreateCategory(dto);

        var category = await context.Category.FirstOrDefaultAsync(c => c.Name == "Doces");

        Assert.NotNull(category);
        Assert.Equal("Doces", category.Name);
        Assert.Equal("Doces", result.Name);
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
    public async Task GetById_ShouldThrowException_WhenCategoryDoesNotExist()
    {
        await using var context = CreateContext();

        var service = CreateService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(999));
    }

    [Fact]
    public async Task UpdateCategory_ShouldUpdateCategory_WhenCategoryExists()
    {
        await using var context = CreateContext();

        context.Category.Add(new Category { CategoryId = 1, Name = "Doces" });
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var dto = new CategoryInputDto { Name = "Doces Finos" };

        var result = await service.UpdateCategory(1, dto);

        Assert.NotNull(result);
        Assert.Equal("Doces Finos", result.Name);

        var updatedCategory = await context.Category.FindAsync(1);
        Assert.NotNull(updatedCategory);
        Assert.Equal("Doces Finos", updatedCategory.Name);
    }

    [Fact]
    public async Task UpdateCategory_ShouldThrowException_WhenCategoryDoesNotExist()
    {
        await using var context = CreateContext();

        var service = CreateService(context);
        var dto = new CategoryInputDto { Name = "Doces" };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateCategory(999, dto));
    }

    [Fact]
    public async Task DeleteCategory_ShouldDeleteCategory_WhenCategoryExists()
    {
        await using var context = CreateContext();

        context.Category.Add(new Category { CategoryId = 1, Name = "Doces" });
        await context.SaveChangesAsync();

        var service = CreateService(context);

        await service.DeleteCategory(1);

        Assert.Null(await context.Category.FindAsync(1));
    }

    [Fact]
    public async Task DeleteCategory_ShouldThrowException_WhenCategoryDoesNotExist()
    {
        await using var context = CreateContext();

        var service = CreateService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteCategory(999));
    }
}