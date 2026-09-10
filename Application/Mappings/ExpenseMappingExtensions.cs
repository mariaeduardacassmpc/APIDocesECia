using ApiDoces.Dtos.Expense;
using Data.Entidades;

namespace ApiDoces.Mappings;

public static class ExpenseMappingExtensions
{
    public static ExpenseDto ToDto(this Expense expense)
    {
        return new ExpenseDto
        {
            Id = expense.ExpenseId,
            Description = expense.Description,
            Value = expense.Value,
            Date = expense.Date
        };
    }

    public static Expense ToEntity(this CreateExpenseDto dto)
    {
        return new Expense
        {
            Description = dto.Description,
            Value = dto.Value,
            Date = dto.Date
        };
    }

    public static void UpdateFromDto(this Expense expense, UpdateExpenseDto dto)
    {
        expense.Description = dto.Description;
        expense.Value = dto.Value;
        expense.Date = dto.Date;
    }
}