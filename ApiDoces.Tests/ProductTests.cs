using ApiDoces.Services;
using Application.Dtos.Product;
using Application.Interfaces;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiDoces.Tests.Services;

public class ProductTests
{
    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static Mock<IImageStorage> CreateImageStorage()
    {
        return new Mock<IImageStorage>();
    }

    private static ProductService CreateService(ApplicationDbContext context, Mock<IImageStorage> imageStorage)
    {
        var logger = new Mock<ILogger<ProductService>>();

        return new ProductService(context, imageStorage.Object, logger.Object);
    }

    [Fact]
    public async Task CreateProduct_ShouldCreateProduct()
    {
        await using var context = CreateContext();
        var imageStorage = CreateImageStorage();
        var service = CreateService(context, imageStorage);

        var dto = new InputProductDto
        {
            Name = "Brigadeiro",
            CategoryId = 1,
            Description = "Brigadeiro tradicional",
            PurchasePrice = 1.50m,
            SalePrice = 3.50m,
            Stock = 20,
            Image = null,
            Active = true
        };

        var result = await service.CreateProduct(dto);

        Assert.NotNull(result);
        Assert.Equal("Brigadeiro", result.Name);

        var product = await context.Product.FirstOrDefaultAsync(p => p.Name == "Brigadeiro");

        Assert.NotNull(product);
        Assert.Equal(1, product.CategoryId);
        Assert.Equal("Brigadeiro tradicional", product.Description);
        Assert.Equal(1.50m, product.PurchasePrice);
        Assert.Equal(3.50m, product.SalePrice);
        Assert.Equal(20, product.Stock);
        Assert.True(product.Active);
    }

    [Fact]
    public async Task CreateProduct_ShouldSaveImage_WhenImageIsBase64()
    {
        await using var context = CreateContext();
        var imageStorage = CreateImageStorage();

        imageStorage.Setup(x => x.SaveFromBase64(It.IsAny<string>()))
            .Returns("images/brigadeiro.jpg");

        var service = CreateService(context, imageStorage);

        var dto = new InputProductDto
        {
            Name = "Brigadeiro",
            CategoryId = 1,
            Description = "Brigadeiro tradicional",
            PurchasePrice = 1.50m,
            SalePrice = 3.50m,
            Stock = 20,
            Image = "data:image/png;base64,ABC123",
            Active = true
        };

        await service.CreateProduct(dto);

        var product = await context.Product
            .FirstOrDefaultAsync(p => p.Name == "Brigadeiro");

        Assert.NotNull(product);
        Assert.Equal("images/brigadeiro.jpg", product.Image);

        imageStorage.Verify(x => x.SaveFromBase64("data:image/png;base64,ABC123"),
            Times.Once);
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnAllProducts()
    {
        using var context = CreateContext();

        var category = new Category
        {
            CategoryId = 1,
            Name = "Doces"
        };

        context.Category.Add(category);

        context.Product.AddRange(
            new Product
            {
                ProductId = 1,
                Name = "Brigadeiro",
                CategoryId = 1,
                SalePrice = 5.00m,
                PurchasePrice = 2.00m,
                Stock = 10,
                Active = true
            },
            new Product
            {
                ProductId = 2,
                Name = "Beijinho",
                CategoryId = 1,
                SalePrice = 4.00m,
                PurchasePrice = 1.50m,
                Stock = 20,
                Active = true
            }
        );

        await context.SaveChangesAsync();

        var imageStorage = new Mock<IImageStorage>();
        var service = CreateService(context, imageStorage);
        var result = await service.GetAllProducts();

        var products = result.ToList();

        Assert.Equal(2, products.Count);
        Assert.Contains(products, p => p.Name == "Brigadeiro");
        Assert.Contains(products, p => p.Name == "Beijinho");
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnEmptyList_WhenThereAreNoProducts()
    {
        await using var context = CreateContext();

        var imageStorage = CreateImageStorage();
        var service = CreateService(context, imageStorage);

        var result = (await service.GetAllProducts()).ToList();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetProductsForSale_ShouldReturnProducts()
    {
        await using var context = CreateContext();

        context.Product.AddRange(
            new Product
            {
                ProductId = 1,
                Name = "Pé de Moça",
                CategoryId = 1,
                Description = "Pé de Moça",
                PurchasePrice = 1.50m,
                SalePrice = 3.50m,
                Stock = 20,
                Active = true
            },
            new Product
            {
                ProductId = 2,
                Name = "Prestigio",
                CategoryId = 1,
                Description = "Prestigio",
                PurchasePrice = 1.20m,
                SalePrice = 3.00m,
                Stock = 15,
                Active = false
            }
        );

        await context.SaveChangesAsync();

        var imageStorage = CreateImageStorage();
        var service = CreateService(context, imageStorage);

        var result = (await service.GetProductsForSale()).ToList();

        Assert.Equal(2, result.Count);

        Assert.Contains(result,
            p => p.Name == "Pé de Moça" &&
                 p.PurchasePrice == 3.50m);

        Assert.Contains(result,
            p => p.Name == "Prestigio" &&
                 p.PurchasePrice == 3.00m);
    }

    [Fact]
    public async Task GetById_ShouldReturnProduct_WhenProductExists()
    {
        using var context = CreateContext();

        var category = new Category
        {
            CategoryId = 1,
            Name = "Doces"
        };

        context.Category.Add(category);

        context.Product.Add(new Product
        {
            ProductId = 1,
            Name = "Brigadeiro",
            CategoryId = 1,
            SalePrice = 5.00m,
            PurchasePrice = 2.00m,
            Stock = 10,
            Active = true
        });

        await context.SaveChangesAsync();

        var imageStorage = new Mock<IImageStorage>();
        var service = CreateService(context, imageStorage);
        var result = await service.GetById(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Brigadeiro", result.Name);
        Assert.Equal(1, result.CategoryId);
        Assert.Equal("Doces", result.Category);
        Assert.Equal(5.00m, result.SalePrice);
        Assert.Equal(2.00m, result.PurchasePrice);
        Assert.Equal(10, result.Stock);
        Assert.True(result.Active);
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenProductDoesNotExist()
    {
        await using var context = CreateContext();

        var imageStorage = CreateImageStorage();
        var service = CreateService(context, imageStorage);

        var result = await service.GetById(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateProduct_ShouldUpdateProduct_WhenProductExists()
    {
        await using var context = CreateContext();

        context.Product.Add(
            new Product
            {
                ProductId = 1,
                Name = "Brigadeiro",
                CategoryId = 1,
                Description = "Descrição antiga",
                PurchasePrice = 1.50m,
                SalePrice = 3.50m,
                Stock = 20,
                Active = true
            });

        await context.SaveChangesAsync();

        var imageStorage = CreateImageStorage();
        var service = CreateService(context, imageStorage);

        var dto = new InputProductDto
        {
            Name = "Brigadeiro Gourmet",
            CategoryId = 2,
            Description = "Descrição atualizada",
            PurchasePrice = 2.00m,
            SalePrice = 5.00m,
            Stock = 30,
            Image = null,
            Active = true
        };

        var result = await service.UpdateProduct(1, dto);

        Assert.NotNull(result);
        Assert.Equal("Brigadeiro Gourmet", result.Name);
        Assert.Equal(2, result.CategoryId);
        Assert.Equal("Descrição atualizada", result.Description);
        Assert.Equal(2.00m, result.PurchasePrice);
        Assert.Equal(5.00m, result.SalePrice);
        Assert.Equal(30, result.Stock);

        var product = await context.Product.FindAsync(1);

        Assert.NotNull(product);
        Assert.Equal("Brigadeiro Gourmet", product.Name);
        Assert.Equal(5.00m, product.SalePrice);
        Assert.Equal(30, product.Stock);
    }

    [Fact]
    public async Task UpdateProduct_ShouldReturnNull_WhenProductDoesNotExist()
    {
        await using var context = CreateContext();

        var imageStorage = CreateImageStorage();
        var service = CreateService(context, imageStorage);

        var dto = new InputProductDto
        {
            Name = "Brigadeiro",
            CategoryId = 1,
            Description = "Brigadeiro",
            PurchasePrice = 1.50m,
            SalePrice = 3.50m,
            Stock = 20,
            Image = null,
            Active = true
        };

        var result = await service.UpdateProduct(999, dto);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateProduct_ShouldSaveImage_WhenImageIsBase64()
    {
        await using var context = CreateContext();

        context.Product.Add(
            new Product
            {
                ProductId = 1,
                Name = "Brigadeiro",
                CategoryId = 1,
                Description = "Brigadeiro",
                PurchasePrice = 1.50m,
                SalePrice = 3.50m,
                Stock = 20,
                Image = "images/old.jpg",
                Active = true
            });

        await context.SaveChangesAsync();

        var imageStorage = CreateImageStorage();

        imageStorage.Setup(x => x.SaveFromBase64(It.IsAny<string>()))
            .Returns("images/new.jpg");

        var service = CreateService(context, imageStorage);

        var dto = new InputProductDto
        {
            Name = "Brigadeiro",
            CategoryId = 1,
            Description = "Brigadeiro",
            PurchasePrice = 1.50m,
            SalePrice = 3.50m,
            Stock = 20,
            Image = "data:image/png;base64,NEWIMAGE",
            Active = true
        };

        var result = await service.UpdateProduct(1, dto);

        Assert.NotNull(result);

        var product = await context.Product.FindAsync(1);

        Assert.NotNull(product);
        Assert.Equal("images/new.jpg", product.Image);

        imageStorage.Verify(x => x.SaveFromBase64("data:image/png;base64,NEWIMAGE"),
            Times.Once);
    }

    [Fact]
    public async Task ToggleActive_ShouldActivateProduct_WhenProductIsInactive()
    {
        await using var context = CreateContext();

        context.Product.Add(
            new Product
            {
                ProductId = 1,
                Name = "Brigadeiro",
                CategoryId = 1,
                Description = "Brigadeiro",
                PurchasePrice = 1.50m,
                SalePrice = 3.50m,
                Stock = 20,
                Active = false
            });

        await context.SaveChangesAsync();

        var imageStorage = CreateImageStorage();
        var service = CreateService(context, imageStorage);

        var result = await service.ToggleActive(1);

        Assert.NotNull(result);
        Assert.True(result.Active);

        var product = await context.Product.FindAsync(1);

        Assert.NotNull(product);
        Assert.True(product.Active);
    }

    [Fact]
    public async Task ToggleActive_ShouldDeactivateProduct_WhenProductIsActive()
    {
        await using var context = CreateContext();

        context.Product.Add(
            new Product
            {
                ProductId = 1,
                Name = "Brigadeiro",
                CategoryId = 1,
                Description = "Brigadeiro",
                PurchasePrice = 1.50m,
                SalePrice = 3.50m,
                Stock = 20,
                Active = true
            });

        await context.SaveChangesAsync();

        var imageStorage = CreateImageStorage();
        var service = CreateService(context, imageStorage);
        var result = await service.ToggleActive(1);

        Assert.NotNull(result);
        Assert.False(result.Active);

        var product = await context.Product.FindAsync(1);

        Assert.NotNull(product);
        Assert.False(product.Active);
    }

    [Fact]
    public async Task ToggleActive_ShouldReturnNull_WhenProductDoesNotExist()
    {
        await using var context = CreateContext();

        var imageStorage = CreateImageStorage();
        var service = CreateService(context, imageStorage);
        var result = await service.ToggleActive(999);

        Assert.Null(result);
    }
}