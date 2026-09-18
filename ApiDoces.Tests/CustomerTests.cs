using ApiDoces.Services;
using Application.Dtos.Customer;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApiDoces.Tests.Services;

public class CustomerTests
{
    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static CustomerService CreateService(ApplicationDbContext context)
    {
        var logger = new Mock<ILogger<CustomerService>>();

        return new CustomerService(context, logger.Object);
    }

    [Fact]
    public async Task CreateCustomer_ShouldCreateCustomer()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        var dto = new InputCustomerDto
        {
            Name = "Maria",
            Email = "maria@email.com",
            Phone = "43999999999",
            Address = "Rua A",
            City = "Londrina",
            Obs = "Cliente teste",
            Active = true
        };

        await service.CreateCustomer(dto);

        var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == "maria@email.com");

        Assert.NotNull(customer);
        Assert.Equal("Maria", customer.Name);
        Assert.Equal("43999999999", customer.Phone);
        Assert.Equal("Rua A", customer.Address);
        Assert.Equal("Londrina", customer.City);
        Assert.Equal("Cliente teste", customer.Obs);
        Assert.True(customer.Active);
    }

    [Fact]
    public async Task UpdateCustomer_ShouldUpdateCustomer_WhenCustomerExists()
    {
        await using var context = CreateContext();

        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Maria",
            Email = "maria@email.com",
            Phone = "43999999999",
            Address = "Rua A",
            City = "Londrina",
            Obs = "Observação antiga",
            Active = true
        };

        context.Customer.Add(customer);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var dto = new InputCustomerDto
        {
            Name = "Maria Eduarda",
            Email = "mariaeduarda@email.com",
            Phone = "43988888888",
            Address = "Rua B",
            City = "Maringá",
            Obs = "Observação atualizada",
            Active = true
        };

        var result = await service.UpdateCustomer(1, dto);

        Assert.NotNull(result);
        Assert.Equal("Maria Eduarda", result.Name);
        Assert.Equal("mariaeduarda@email.com", result.Email);
        Assert.Equal("43988888888", result.Phone);
        Assert.Equal("Rua B", result.Address);
        Assert.Equal("Maringá", result.City);
        Assert.Equal("Observação atualizada", result.Obs);
        Assert.True(result.Active);
    }

    [Fact]
    public async Task UpdateCustomer_ShouldThrowException_WhenCustomerDoesNotExist()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        var dto = new InputCustomerDto
        {
            Name = "Maria",
            Email = "maria@email.com",
            Phone = "43999999999",
            Address = "Rua A",
            City = "Londrina",
            Obs = "",
            Active = true
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateCustomer(999, dto));
    }

    [Fact]
    public async Task GetById_ShouldReturnCustomer_WhenCustomerExists()
    {
        await using var context = CreateContext();

        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Maria",
            Email = "maria@email.com",
            Phone = "43999999999",
            Address = "Rua A",
            City = "Londrina",
            Obs = "Cliente teste",
            Active = true
        };

        context.Customer.Add(customer);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.GetById(1);

        Assert.NotNull(result);
        Assert.Equal("Maria", result.Name);
        Assert.Equal("maria@email.com", result.Email);
        Assert.Equal("Londrina", result.City);
        Assert.True(result.Active);
    }

    [Fact]
    public async Task GetById_ShouldThrowException_WhenCustomerDoesNotExist()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(999));
    }

    [Fact]
    public async Task ToggleActive_ShouldActivateCustomer_WhenCustomerIsInactive()
    {
        await using var context = CreateContext();

        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Maria",
            Email = "maria@email.com",
            Phone = "43999999999",
            Address = "Rua A",
            City = "Londrina",
            Obs = "",
            Active = false
        };

        context.Customer.Add(customer);
        await context.SaveChangesAsync();

        var service = CreateService(context);
        var result = await service.ToggleActive(1);

        Assert.NotNull(result);
        Assert.True(result.Active);

        var customerInDatabase = await context.Customer.FindAsync(1);
        Assert.True(customerInDatabase!.Active);
    }

    [Fact]
    public async Task ToggleActive_ShouldDeactivateCustomer_WhenCustomerIsActive()
    {
        await using var context = CreateContext();

        var customer = new Customer
        {
            CustomerId = 1,
            Name = "Maria",
            Email = "maria@email.com",
            Phone = "43999999999",
            Address = "Rua A",
            City = "Londrina",
            Obs = "",
            Active = true
        };

        context.Customer.Add(customer);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.ToggleActive(1);

        Assert.NotNull(result);
        Assert.False(result.Active);

        var customerInDatabase = await context.Customer.FindAsync(1);
        Assert.False(customerInDatabase!.Active);
    }

    [Fact]
    public async Task ToggleActive_ShouldThrowException_WhenCustomerDoesNotExist()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ToggleActive(999));
    }
}