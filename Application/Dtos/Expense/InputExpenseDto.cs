namespace Application.Dtos.Expense;

public class InputExpenseDto
{
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime Date { get; set; }
}
