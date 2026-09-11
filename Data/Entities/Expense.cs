namespace Data.Entities;

public class Expense
{
    public int ExpenseId { get; set; }
    public required string Description { get; set; }
    public required decimal Value { get; set; }
    public required DateTime Date { get; set; }
}