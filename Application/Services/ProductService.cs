using ApiDoces.Mappings;
using Application.Dtos.Customer;
using Application.Dtos.Product;
using Application.Interfaces;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApiDoces.Services;

public class ProductService(ApplicationDbContext context, IImageStorage imageStorage, ILogger<ProductService> logger)
{
    public async Task<ProductDto> CreateProduct(InputProductDto dto)
    {
        logger.LogInformation("Criando produto: {ProductName}", dto.Name);

        var product = dto.ToEntity();

        if (!string.IsNullOrEmpty(dto.Image) && dto.Image.StartsWith("data:image"))
            product.Image = imageStorage.SaveFromBase64(dto.Image);

        context.Add(product);
        await context.SaveChangesAsync();

        logger.LogInformation("Produto criado com sucesso. Id: {ProductId}", product.ProductId);

        return product.ToDto();
    }

    public async Task<IEnumerable<ProductDto>> GetAllProducts()
    {
        logger.LogInformation("Buscando todos os produtos");

        var products = await context.Product.ToListAsync();

        logger.LogInformation("Foram encontrados {Count} produtos", products.Count);

        return products.Select(p => p.ToDto());
    }

    public async Task<IEnumerable<ProductDto>> GetProductsForSale()
    {
        logger.LogInformation("Buscando produtos disponíveis para venda");

        var products = await context.Product
            .Select(p => new ProductDto
            {
                Id = p.ProductId,
                Name = p.Name,
                PurchasePrice = p.SalePrice
            })
            .ToListAsync();

        logger.LogInformation("Foram encontrados {Count} produtos para venda", products.Count);

        return products;
    }

    public async Task<ProductDto?> GetById(int id)
    {
        logger.LogInformation("Buscando produto por Id: {ProductId}", id);

        var product = await context.Product.FindAsync(id);

        if (product == null)
        {
            logger.LogWarning("Produto não encontrado. Id: {ProductId}", id);
            return null;
        }

        return product.ToDto();
    }

    public async Task<ProductDto?> UpdateProduct(int id, InputProductDto dto)
    {
        logger.LogInformation("Atualizando produto. Id: {ProductId}", id);

        var existingProduct = await context.Product.FindAsync(id);

        if (existingProduct == null)
        {
            logger.LogWarning("Produto não encontrado para atualização. Id: {ProductId}", id);
            return null;
        }

        dto.UpdateEntity(existingProduct);

        if (!string.IsNullOrEmpty(dto.Image) && dto.Image.StartsWith("data:image"))
            existingProduct.Image = imageStorage.SaveFromBase64(dto.Image);

        await context.SaveChangesAsync();

        logger.LogInformation("Produto atualizado com sucesso. Id: {ProductId}", id);

        return existingProduct.ToDto();
    }

    public async Task<ProductDto?> ToggleActive(int id)
    {
        logger.LogInformation("Alterando status do produto. Id: {ProductId}", id);

        var product = await context.Product.FindAsync(id);

        if (product == null)
        {
            logger.LogWarning("Produto não encontrado para alteração de status. Id: {ProductId}", id);
            return null;
        }

        product.Active = !product.Active;

        await context.SaveChangesAsync();

        logger.LogInformation("Status do produto alterado. Id: {ProductId}, Ativo: {Active}", id, product.Active);

        return product.ToDto();
    }
}