using ApiDoces.Services;
using Application.Dtos.Expense;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

public class ExpenseTests
{
    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static ExpenseService CreateService(ApplicationDbContext context)
    {
        var logger = new Mock<ILogger<ExpenseService>>();

        return new ExpenseService(context, logger.Object);
    }

    [Fact]
    public async Task CreateExpense_ShouldCreateExpense()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        var dto = new InputExpenseDto
        {
            Description = "Embalagens",
            Value = 120m,
            Date = new DateTime(2026, 9, 1)
        };

        var result = await service.CreateExpense(dto);

        Assert.NotNull(result);
        Assert.Equal("Embalagens", result.Description);
        Assert.Equal(120m, result.Value);

        var expense = await context.Expense.FirstOrDefaultAsync(e => e.Description == "Embalagens");

        Assert.NotNull(expense);
        Assert.Equal(120m, expense.Value);
        Assert.Equal(new DateTime(2026, 9, 1), expense.Date);
    }

    [Fact]
    public async Task GetAllExpenses_ShouldReturnExpensesOrderedByDateDescending()
    {
        await using var context = CreateContext();

        context.Expense.AddRange(
            new Expense
            {
                ExpenseId = 1,
                Description = "Embalagens",
                Value = 120m,
                Date = new DateTime(2026, 8, 1)
            },
            new Expense
            {
                ExpenseId = 2,
                Description = "Gasolina",
                Value = 300m,
                Date = new DateTime(2026, 9, 1)
            }
        );

        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = (await service.GetAllExpenses()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("Gasolina", result[0].Description);
        Assert.Equal("Embalagens", result[1].Description);
    }

    [Fact]
    public async Task GetAllExpenses_ShouldReturnEmptyList_WhenThereAreNoExpenses()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        var result = (await service.GetAllExpenses()).ToList();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetById_ShouldReturnExpense_WhenExpenseExists()
    {
        await using var context = CreateContext();

        context.Expense.Add(new Expense
        {
            ExpenseId = 1,
            Description = "Embalagens",
            Value = 120m,
            Date = new DateTime(2026, 9, 1)
        });

        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.GetById(1);

        Assert.NotNull(result);
        Assert.Equal("Embalagens", result.Description);
        Assert.Equal(120m, result.Value);
    }

    [Fact]
    public async Task GetById_ShouldThrow_WhenExpenseDoesNotExist()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetById(999));
    }

    [Fact]
    public async Task UpdateExpense_ShouldThrow_WhenExpenseDoesNotExist()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        var dto = new InputExpenseDto
        {
            Description = "Embalagens",
            Value = 100m,
            Date = new DateTime(2026, 9, 1)
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateExpense(999, dto));
    }

    [Fact]
    public async Task DeleteExpense_ShouldDeleteExpense_WhenExpenseExists()
    {
        await using var context = CreateContext();

        context.Expense.Add(new Expense
        {
            ExpenseId = 1,
            Description = "Embalagens",
            Value = 120m,
            Date = new DateTime(2026, 9, 1)
        });

        await context.SaveChangesAsync();

        var service = CreateService(context);

        await service.DeleteExpense(1);

        Assert.Null(await context.Expense.FindAsync(1));
    }

    [Fact]
    public async Task DeleteExpense_ShouldThrow_WhenExpenseDoesNotExist()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteExpense(999));
    }

    [Fact]
    public async Task GetFinancialSummary_ShouldCalculateRevenueExpensesAndNetProfit()
    {
        await using var context = CreateContext();

        context.Sale.AddRange(
            new Sale
            {
                SaleId = 1,
                SaleDate = new DateTime(2026, 9, 10),
                TotalAmount = 500m,
                PaymentMethod = "pix",
                Items = new List<SaleItem>()
            },
            new Sale
            {
                SaleId = 2,
                SaleDate = new DateTime(2026, 9, 20),
                TotalAmount = 300m,
                PaymentMethod = "cartao",
                Items = new List<SaleItem>()
            },
            new Sale
            {
                SaleId = 3,
                SaleDate = new DateTime(2026, 8, 15),
                TotalAmount = 999m,
                PaymentMethod = "dinheiro",
                Items = new List<SaleItem>()
            }
        );

        context.Expense.AddRange(
            new Expense
            {
                ExpenseId = 1,
                Description = "Aluguel",
                Value = 200m,
                Date = new DateTime(2026, 9, 1)
            },
            new Expense
            {
                ExpenseId = 2,
                Description = "Energia",
                Value = 100m,
                Date = new DateTime(2026, 8, 1)
            }
        );

        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.GetFinancialSummary(9, 2026);

        Assert.Equal(800m, result.TotalRevenue);
        Assert.Equal(200m, result.TotalExpenses);
        Assert.Equal(600m, result.NetProfit);
    }

    [Fact]
    public async Task GetFinancialSummary_ShouldReturnZeroed_WhenThereIsNoDataForThePeriod()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        var result = await service.GetFinancialSummary(9, 2026);

        Assert.Equal(0m, result.TotalRevenue);
        Assert.Equal(0m, result.TotalExpenses);
        Assert.Equal(0m, result.NetProfit);
    }
}