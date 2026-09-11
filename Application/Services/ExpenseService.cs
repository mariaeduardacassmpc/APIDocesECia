using ApiDoces.Dtos.Expense;
using ApiDoces.Mappings;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApiDoces.Services;

public class ExpenseService(ApplicationDbContext context, ILogger<ExpenseService> logger)
{
    public async Task<ExpenseDto> CreateExpense(CreateExpenseDto dto)
    {
        logger.LogInformation("Criando nova despesa");

        var expense = dto.ToEntity();

        context.Expense.Add(expense);
        await context.SaveChangesAsync();

        logger.LogInformation("Despesa criada com sucesso. Id: {ExpenseId}", expense.ExpenseId);

        return expense.ToDto();
    }

    public async Task<IEnumerable<ExpenseDto>> GetAllExpenses()
    {
        logger.LogInformation("Buscando todas as despesas");

        var expenses = await context.Expense
            .OrderByDescending(x => x.Date)
            .ToListAsync();

        logger.LogInformation("Foram encontradas {Count} despesas", expenses.Count);

        return expenses.Select(x => x.ToDto());
    }

    public async Task<ExpenseDto?> GetById(int id)
    {
        logger.LogInformation("Buscando despesa por Id: {ExpenseId}", id);

        var expense = await context.Expense.FindAsync(id);

        if (expense == null)
        {
            logger.LogWarning("Despesa não encontrada. Id: {ExpenseId}", id);
            return null;
        }

        return expense.ToDto();
    }

    public async Task<ExpenseDto?> UpdateExpense(int id, UpdateExpenseDto dto)
    {
        logger.LogInformation("Atualizando despesa. Id: {ExpenseId}", id);

        var existingExpense = await context.Expense.FindAsync(id);

        if (existingExpense == null)
        {
            logger.LogWarning("Despesa não encontrada para atualização. Id: {ExpenseId}", id);
            return null;
        }

        existingExpense.UpdateFromDto(dto);
        await context.SaveChangesAsync();

        logger.LogInformation("Despesa atualizada com sucesso. Id: {ExpenseId}", id);

        return existingExpense.ToDto();
    }

    public async Task<bool> DeleteExpense(int id)
    {
        logger.LogInformation("Excluindo despesa. Id: {ExpenseId}", id);

        var expense = await context.Expense.FindAsync(id);

        if (expense == null)
        {
            logger.LogWarning("Despesa não encontrada para exclusão. Id: {ExpenseId}", id);
            return false;
        }

        context.Expense.Remove(expense);
        await context.SaveChangesAsync();

        logger.LogInformation("Despesa excluída com sucesso. Id: {ExpenseId}", id);

        return true;
    }

    public async Task<FinancialSummaryDto> GetFinancialSummary(int month, int year)
    {
        logger.LogInformation("Gerando resumo financeiro. Mês: {Month}, Ano: {Year}", month, year);

        var dateStart = new DateTime(year, month, 1);
        var dateEnd = dateStart.AddMonths(1);

        var totalRevenue = await context.Sale
            .Where(x => x.SaleDate >= dateStart && x.SaleDate < dateEnd)
            .SumAsync(x => (decimal?)x.TotalAmount) ?? 0m;

        var totalExpenses = await context.Expense
            .Where(x => x.Date >= dateStart && x.Date < dateEnd)
            .SumAsync(x => (decimal?)x.Value) ?? 0m;

        var result = new FinancialSummaryDto
        {
            TotalRevenue = totalRevenue,
            TotalExpenses = totalExpenses,
            NetProfit = totalRevenue - totalExpenses
        };

        logger.LogInformation(
            "Resumo financeiro gerado. Receita: {Revenue}, Despesas: {Expenses}, Lucro líquido: {NetProfit}",
            result.TotalRevenue, result.TotalExpenses, result.NetProfit);

        return result;
    }
}