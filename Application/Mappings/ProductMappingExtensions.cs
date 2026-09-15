using Application.Dtos.Product;
using Data.Entities;

namespace ApiDoces.Mappings;

public static class ProductMappingExtensions
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.ProductId,
            Name = product.Name,
            CategoryId = product.CategoryId,
            Category = product.Category?.Name ?? string.Empty,
            Description = product.Description,
            PurchasePrice = product.PurchasePrice,
            SalePrice = product.SalePrice,
            Stock = product.Stock,
            Image = product.Image,
            Active = product.Active
        };
    }

    public static Product ToEntity(this InputProductDto dto)
    {
        return new Product
        {
            Name = dto.Name,
            CategoryId = dto.CategoryId,
            Description = dto.Description,
            PurchasePrice = dto.PurchasePrice,
            SalePrice = dto.SalePrice,
            Stock = dto.Stock,
            Image = dto.Image ?? string.Empty,
            Active = dto.Active
        };
    }

    public static void UpdateEntity(this InputProductDto dto, Product product)
    {
        product.Name = dto.Name;
        product.CategoryId = dto.CategoryId;
        product.Description = dto.Description;
        product.SalePrice = dto.SalePrice;
        product.PurchasePrice = dto.PurchasePrice;
        product.Stock = dto.Stock;
        product.Active = dto.Active;
    }
}