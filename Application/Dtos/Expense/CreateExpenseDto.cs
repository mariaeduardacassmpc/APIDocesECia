namespace ApiDoces.Dtos.Expense;

public class CreateExpenseDto
{
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime Date { get; set; }
}
