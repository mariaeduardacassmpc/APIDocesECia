using ApiDoces.Dtos.Product;
using Data.Entidades;

namespace ApiDoces.Mappings;

public static class ProductMappingExtensions
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.ProductId,
            Name = product.Name,
            Category = product.Category,
            Description = product.Description,
            PurchasePrice = product.PurchasePrice,
            SalePrice = product.SalePrice,
            Stock = product.Stock,
            Image = product.Image
        };
    }

    public static Product ToEntity(this CreateProductDto dto)
    {
        return new Product
        {
            Name = dto.Name,
            Category = dto.Category,
            Description = dto.Description,
            PurchasePrice = dto.PurchasePrice,
            SalePrice = dto.SalePrice,
            Stock = dto.Stock,
            Image = dto.Image ?? string.Empty
        };
    }

    public static void UpdateFromDto(this Product product, UpdateProductDto dto)
    {
        product.Name = dto.Name;
        product.Category = dto.Category;
        product.Description = dto.Description;
        product.PurchasePrice = dto.PurchasePrice;
        product.SalePrice = dto.SalePrice;
        product.Stock = dto.Stock;
    }
}