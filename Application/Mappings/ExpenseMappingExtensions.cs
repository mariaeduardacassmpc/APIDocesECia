using Application.Dtos.Expense;
using Data.Entities;

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

    public static Expense ToEntity(this InputExpenseDto dto)
    {
        return new Expense
        {
            Description = dto.Description,
            Value = dto.Value,
            Date = dto.Date
        };
    }
}