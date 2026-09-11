namespace Data.Entities;

public class Sale
{
    public int SaleId { get; set; }
    public string? Description { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public required string PaymentMethod { get; set; } = string.Empty;
    public required decimal TotalAmount { get; set; }
    public required DateTime SaleDate { get; set; }
    public required List<SaleItem> Items { get; set; } = new();
}

public class SaleItem
{
    public int SaleItemId { get; set; }
    public int SaleId { get; set; }
    public Sale Sale { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public required int Quantity { get; set; }
    public required decimal UnitPrice { get; set; }
    public decimal Subtotal => Quantity * UnitPrice;
}