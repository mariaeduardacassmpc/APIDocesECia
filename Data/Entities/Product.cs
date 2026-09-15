namespace Data.Entities;

public class Product
{
    public int ProductId { get; set; }
    public required string Name { get; set; } 
    public string Description { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public string Image { get; set; } = string.Empty;
    public required decimal SalePrice { get; set; } 
    public required decimal PurchasePrice { get; set; }
    public required int Stock { get; set; }
    public bool Active { get; set; } = true;
}
    