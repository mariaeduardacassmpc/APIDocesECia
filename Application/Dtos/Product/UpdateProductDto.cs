namespace ApiDoces.Dtos.Product;

public class UpdateProductDto
{
    public required string Name { get; set; }
    public required string Category { get; set; }
    public string Description { get; set; } = string.Empty;
    public required decimal PurchasePrice { get; set; }
    public required decimal SalePrice { get; set; }
    public required int Stock { get; set; }
    public string? Image { get; set; }
}