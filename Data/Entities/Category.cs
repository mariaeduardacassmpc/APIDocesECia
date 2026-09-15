namespace Data.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public required string Name { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();   
}
    