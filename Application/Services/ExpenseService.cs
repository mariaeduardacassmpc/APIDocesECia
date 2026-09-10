using ApiDoces.Dtos.Expense;
using ApiDoces.Mappings;
using Data;
using Microsoft.EntityFrameworkCore;

namespace ApiDoces.Services;

public class ExpenseService(ApplicationDbContext context)
{
    public async Task<ExpenseDto> CreateExpense(CreateExpenseDto dto)
    {
        var expense = dto.ToEntity();

        context.Expense.Add(expense);
        await context.SaveChangesAsync();

        return expense.ToDto();
    }

    public async Task<IEnumerable<ExpenseDto>> GetAllExpenses()
    {
        var expenses = await context.Expense
            .OrderByDescending(x => x.Date)
            .ToListAsync();

        return expenses.Select(x => x.ToDto());
    }

    public async Task<ExpenseDto?> GetById(int id)
    {
        var expense = await context.Expense.FindAsync(id);
        return expense?.ToDto();
    }

    public async Task<ExpenseDto?> UpdateExpense(int id, UpdateExpenseDto dto)
    {
        var existingExpense = await context.Expense.FindAsync(id);

        if (existingExpense == null)
            return null;

        existingExpense.UpdateFromDto(dto);
        await context.SaveChangesAsync();

        return existingExpense.ToDto();
    }

    public async Task<bool> DeleteExpense(int id)
    {
        var expense = await context.Expense.FindAsync(id);
        if (expense == null)
            return false;

        context.Expense.Remove(expense);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<FinancialSummaryDto> GetFinancialSummary(int month, int year)
    {
        var dateStart = new DateTime(year, month, 1);
        var dateEnd = dateStart.AddMonths(1);

        var totalRevenue = await context.Sale
            .Where(x => x.SaleDate >= dateStart && x.SaleDate < dateEnd)
            .SumAsync(x => (decimal?)x.TotalAmount) ?? 0m;

        var totalExpenses = await context.Expense
            .Where(x => x.Date >= dateStart && x.Date < dateEnd)
            .SumAsync(x => (decimal?)x.Value) ?? 0m;

        return new FinancialSummaryDto
        {
            TotalRevenue = totalRevenue,
            TotalExpenses = totalExpenses,
            NetProfit = totalRevenue - totalExpenses
        };
    }
}