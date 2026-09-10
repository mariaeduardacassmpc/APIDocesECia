namespace ApiDoces.Dtos.Expense;

public class UpdateExpenseDto
{
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime Date { get; set; }
}