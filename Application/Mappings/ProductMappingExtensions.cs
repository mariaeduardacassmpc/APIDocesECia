using Application.Dtos.Customer;
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
            Category = product.Category,
            Description = product.Description,
            PurchasePrice = product.PurchasePrice,
            SalePrice = product.SalePrice,
            Stock = product.Stock,
            Image = product.Image
        };
    }

    public static Product ToEntity(this InputProductDto dto)
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

    public static void UpdateEntity(this InputProductDto dto, Product product)
    {
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Category = dto.Category;
        product.SalePrice = dto.SalePrice;
        product.PurchasePrice = dto.PurchasePrice;
        product.Stock = dto.Stock;
        product.Active = dto.Active;
    }
}