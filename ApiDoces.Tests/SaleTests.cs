using ApiDoces.Services;
using Application.Dtos.Sale;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiDoces.Tests.Services;

public class SaleServiceTests
{
    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static SaleService CreateService(ApplicationDbContext context)
    {
        var logger = new Mock<ILogger<SaleService>>();

        return new SaleService(context, logger.Object);
    }

    [Fact]
    public async Task CreateSales_ShouldCreateSale()
    {
        using var context = CreateContext();
        var service = CreateService(context);

        var dto = new InputSaleDto
        {
            Description = "Venda de doces",
            CustomerId = 1,
            PaymentMethod = "Pix",
            TotalAmount = 25.00m,
            SaleDate = DateTime.Now,
            Items = new List<CreateSaleItemDto>
            {
                new()
                {
                    ProductId = 1,
                    Quantity = 2,
                    UnitPrice = 12.50m
                }
            }
        };

        await service.CreateSales(dto);

        var sale = await context.Sale
            .Include(s => s.Items)
            .FirstOrDefaultAsync();

        Assert.NotNull(sale);
        Assert.Equal("Venda de doces", sale.Description);
        Assert.Equal(1, sale.CustomerId);
        Assert.Equal("Pix", sale.PaymentMethod);
        Assert.Equal(25.00m, sale.TotalAmount);
        Assert.Single(sale.Items);
    }

    [Fact]
    public async Task GetSaleById_ShouldReturnSale_WhenSaleExists()
    {
        using var context = CreateContext();

        var sale = new Sale
        {
            SaleId = 1,
            Description = "Venda teste",
            CustomerId = 1,
            PaymentMethod = "Pix",
            TotalAmount = 30.00m,
            SaleDate = new DateTime(2026, 9, 15),
            Items = new List<SaleItem>()
        };

        context.Sale.Add(sale);
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.GetSaleById(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Venda teste", result.Description);
        Assert.Equal(1, result.CustomerId);
        Assert.Equal("Pix", result.PaymentMethod);
        Assert.Equal(30.00m, result.TotalAmount);
    }

    [Fact]
    public async Task GetSaleById_ShouldThrow_WhenSaleDoesNotExist()
    {
        using var context = CreateContext();
        var service = CreateService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetSaleById(999));
    }

    [Fact]
    public async Task GetAllSales_ShouldReturnAllSales()
    {
        using var context = CreateContext();

        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Maria",
            Phone = "999999999",
            City = "Londrina",
            Address = "Rua Teste",
            Active = true,
            Email = "maria@email.com"
        };

        var product = new Product
        {
            ProductId = 1,
            Name = "Brigadeiro",
            CategoryId = 1,
            SalePrice = 5.00m,
            PurchasePrice = 2.00m,
            Stock = 10,
            Active = true
        };

        context.Customer.Add(customer);
        context.Product.Add(product);

        context.Sale.AddRange(
            new Sale
            {
                SaleId = 1,
                CustomerId = 1,
                PaymentMethod = "Pix",
                TotalAmount = 20.00m,
                SaleDate = new DateTime(2026, 9, 14),
                Items = new List<SaleItem>
                {
                    new()
                    {
                        ProductId = 1,
                        Quantity = 4,
                        UnitPrice = 5.00m
                    }
                }
            },
            new Sale
            {
                SaleId = 2,
                CustomerId = 1,
                PaymentMethod = "Dinheiro",
                TotalAmount = 30.00m,
                SaleDate = new DateTime(2026, 9, 15),
                Items = new List<SaleItem>
                {
                    new()
                    {
                        ProductId = 1,
                        Quantity = 6,
                        UnitPrice = 5.00m
                    }
                }
            }
        );

        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.GetAllSales(new SaleFilterDto());

        var sales = result.ToList();

        Assert.Equal(2, sales.Count);
    }

    [Fact]
    public async Task GetAllSales_ShouldFilterByPaymentMethod()
    {
        using var context = CreateContext();

        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Maria",
            Phone = "999999999",
            City = "Londrina",
            Address = "Rua Teste",
            Active = true,
            Email = "maria@email.com"
        };

        context.Customer.Add(customer);

        context.Sale.AddRange(
            new Sale
            {
                SaleId = 1,
                CustomerId = 1,
                PaymentMethod = "Pix",
                TotalAmount = 20.00m,
                SaleDate = new DateTime(2026, 9, 14),
                Items = new List<SaleItem>()
            },
            new Sale
            {
                SaleId = 2,
                CustomerId = 1,
                PaymentMethod = "Dinheiro",
                TotalAmount = 30.00m,
                SaleDate = new DateTime(2026, 9, 15),
                Items = new List<SaleItem>()
            }
        );

        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.GetAllSales(new SaleFilterDto
        {
            Payment = "Pix"
        });

        var sales = result.ToList();

        Assert.Single(sales);
        Assert.Equal("Pix", sales[0].PaymentMethod);
    }

    [Fact]
    public async Task GetAllSales_ShouldSearchByCustomerName()
    {
        using var context = CreateContext();

        context.Customer.AddRange(
            new Customer
            {
                CustomerId = 1,
                Name = "Maria",
                Phone = "999999999",
                City = "Londrina",
                Address = "Rua A",
                Active = true,
                Email = "maria@email.com"
            },
            new Customer
            {
                CustomerId = 2,
                Name = "João",
                Phone = "988888888",
                City = "Londrina",
                Address = "Rua B",
                Active = true,
                Email = "joao@email.com"
            });

        context.Sale.AddRange(
            new Sale
            {
                SaleId = 1,
                CustomerId = 1,
                PaymentMethod = "Pix",
                TotalAmount = 20.00m,
                SaleDate = new DateTime(2026, 9, 14),
                Items = new List<SaleItem>()
            },
            new Sale
            {
                SaleId = 2,
                CustomerId = 2,
                PaymentMethod = "Pix",
                TotalAmount = 30.00m,
                SaleDate = new DateTime(2026, 9, 15),
                Items = new List<SaleItem>()
            });

        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.GetAllSales(new SaleFilterDto
        {
            Search = "Maria"
        });

        var sales = result.ToList();

        Assert.Single(sales);
        Assert.Equal(1, sales[0].CustomerId);
    }

    [Fact]
    public async Task GetAllSales_ShouldFilterByDateRange()
    {
        using var context = CreateContext();

        context.Customer.Add(new Customer
        {
            CustomerId = 1,
            Name = "Maria",
            Phone = "999999999",
            City = "Londrina",
            Address = "Rua Teste",
            Active = true,
            Email = "maria@email.com"
        });

        context.Sale.AddRange(
            new Sale
            {
                SaleId = 1,
                CustomerId = 1,
                PaymentMethod = "Pix",
                TotalAmount = 20.00m,
                SaleDate = new DateTime(2026, 9, 10),
                Items = new List<SaleItem>()
            },
            new Sale
            {
                SaleId = 2,
                CustomerId = 1,
                PaymentMethod = "Pix",
                TotalAmount = 30.00m,
                SaleDate = new DateTime(2026, 9, 15),
                Items = new List<SaleItem>()
            },
            new Sale
            {
                SaleId = 3,
                CustomerId = 1,
                PaymentMethod = "Pix",
                TotalAmount = 40.00m,
                SaleDate = new DateTime(2026, 9, 20),
                Items = new List<SaleItem>()
            }
        );

        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.GetAllSales(new SaleFilterDto
        {
            DateStart = new DateTime(2026, 9, 14),
            DateEnd = new DateTime(2026, 9, 16)
        });

        var sales = result.ToList();

        Assert.Single(sales);
        Assert.Equal(2, sales[0].Id);
    }

    [Fact]
    public async Task GetAllSales_ShouldOrderByOldest_WhenSortByIsAntigos()
    {
        using var context = CreateContext();

        context.Customer.Add(new Customer
        {
            CustomerId = 1,
            Name = "Maria",
            Phone = "999999999",
            City = "Londrina",
            Address = "Rua Teste",
            Active = true,
            Email = "maria@email.com"
        });

        context.Sale.AddRange(
            new Sale
            {
                SaleId = 1,
                CustomerId = 1,
                PaymentMethod = "Pix",
                TotalAmount = 20.00m,
                SaleDate = new DateTime(2026, 9, 15),
                Items = new List<SaleItem>()
            },
            new Sale
            {
                SaleId = 2,
                CustomerId = 1,
                PaymentMethod = "Pix",
                TotalAmount = 30.00m,
                SaleDate = new DateTime(2026, 9, 10),
                Items = new List<SaleItem>()
            }
        );

        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.GetAllSales(new SaleFilterDto
        {
            SortBy = "antigos"
        });

        var sales = result.ToList();

        Assert.Equal(2, sales[0].Id);
        Assert.Equal(1, sales[1].Id);
    }

    [Fact]
    public async Task GetAllSales_ShouldOrderByHighestAmount_WhenSortByIsMaior()
    {
        using var context = CreateContext();

        context.Customer.Add(new Customer
        {
            CustomerId = 1,
            Name = "Maria",
            Phone = "999999999",
            City = "Londrina",
            Address = "Rua Teste",
            Active = true,
            Email = "maria@email.com"
        });

        context.Sale.AddRange(
            new Sale
            {
                SaleId = 1,
                CustomerId = 1,
                PaymentMethod = "Pix",
                TotalAmount = 20.00m,
                SaleDate = DateTime.Now,
                Items = new List<SaleItem>()
            },
            new Sale
            {
                SaleId = 2,
                CustomerId = 1,
                PaymentMethod = "Pix",
                TotalAmount = 50.00m,
                SaleDate = DateTime.Now,
                Items = new List<SaleItem>()
            }
        );

        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.GetAllSales(new SaleFilterDto
        {
            SortBy = "maior"
        });

        var sales = result.ToList();

        Assert.Equal(2, sales[0].Id);
        Assert.Equal(50.00m, sales[0].TotalAmount);
    }

    [Fact]
    public async Task UpdateSale_ShouldUpdateSale_WhenSaleExists()
    {
        using var context = CreateContext();

        context.Customer.Add(new Customer
        {
            CustomerId = 1,
            Name = "Maria",
            Phone = "999999999",
            City = "Londrina",
            Address = "Rua Teste",
            Active = true,
            Email = "maria@email.com"
        });

        var sale = new Sale
        {
            SaleId = 1,
            CustomerId = 1,
            PaymentMethod = "Pix",
            TotalAmount = 20.00m,
            SaleDate = new DateTime(2026, 9, 15),
            Items = new List<SaleItem>
            {
                new()
                {
                    ProductId = 1,
                    Quantity = 2,
                    UnitPrice = 10.00m
                }
            }
        };

        context.Sale.Add(sale);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var dto = new InputSaleDto
        {
            Description = "Venda atualizada",
            CustomerId = 1,
            PaymentMethod = "Cartão",
            TotalAmount = 50.00m,
            SaleDate = new DateTime(2026, 9, 16),
            Items = new List<CreateSaleItemDto>
            {
                new()
                {
                    ProductId = 2,
                    Quantity = 5,
                    UnitPrice = 10.00m
                }
            }
        };

        var result = await service.UpdateSale(1, dto);

        Assert.NotNull(result);
        Assert.Equal("Venda atualizada", result.Description);
        Assert.Equal("Cartão", result.PaymentMethod);
        Assert.Equal(50.00m, result.TotalAmount);
    }

    [Fact]
    public async Task UpdateSale_ShouldThrow_WhenSaleDoesNotExist()
    {
        using var context = CreateContext();
        var service = CreateService(context);

        var dto = new InputSaleDto
        {
            Description = "Venda",
            CustomerId = 1,
            PaymentMethod = "Pix",
            TotalAmount = 20.00m,
            SaleDate = DateTime.Now,
            Items = new List<CreateSaleItemDto>()
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateSale(999, dto));
    }
}