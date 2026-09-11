namespace Data.Entities;

public class Product
{
    public int ProductId { get; set; }
    public required string Name { get; set; } 
    public string Description { get; set; } = string.Empty;
    public required string Category { get; set; } 
    public string Image { get; set; } = string.Empty;
    public required decimal SalePrice { get; set; } 
    public required decimal PurchasePrice { get; set; }
    public required int Stock { get; set; }
}
