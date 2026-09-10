using Data;
using ApiDoces.Mappings;
using ApiDoces.Dtos.Product;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace ApiDoces.Services;

public class ProductService(ApplicationDbContext context, IImageStorage imageStorage)
{
    public async Task<ProductDto> CreateProduct(CreateProductDto dto)
    {
        var product = dto.ToEntity();

        if (!string.IsNullOrEmpty(dto.Image) && dto.Image.StartsWith("data:image"))
            product.Image = imageStorage.SaveFromBase64(dto.Image);

        context.Add(product);
        await context.SaveChangesAsync();

        return product.ToDto();
    }

    public async Task<IEnumerable<ProductDto>> GetAllProducts()
    {
        var products = await context.Product.ToListAsync();
        return products.Select(p => p.ToDto());
    }

    public async Task<IEnumerable<ProductDto>> GetProductsForSale()
    {
        return await context.Product
            .Select(p => new ProductDto
            {
                Id = p.ProductId,
                Name = p.Name,
                PurchasePrice = p.SalePrice
            })
            .ToListAsync();
    }

    public async Task<ProductDto?> GetById(int id)
    {
        var product = await context.Product.FindAsync(id);
        return product?.ToDto();
    }

    public async Task<ProductDto?> UpdateProduct(int id, UpdateProductDto dto)
    {
        var existingProduct = await context.Product.FindAsync(id);

        if (existingProduct == null)
            return null;

        existingProduct.UpdateFromDto(dto);

        if (!string.IsNullOrEmpty(dto.Image) && dto.Image.StartsWith("data:image"))
            existingProduct.Image = imageStorage.SaveFromBase64(dto.Image);

        await context.SaveChangesAsync();
        return existingProduct.ToDto();
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var product = await context.Product.FindAsync(id);
        if (product == null)
            return false;

        context.Product.Remove(product);
        await context.SaveChangesAsync();
        return true;
    }
}