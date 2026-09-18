using Application.Dtos.Category;
using ApiDoces.Mappings;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApiDoces.Services;

public class CategoryService(ApplicationDbContext context, ILogger<CategoryService> logger)
{
    public async Task<CategoryDto> CreateCategory(CategoryInputDto dto)
    {
        logger.LogInformation("Criando categoria");

        var category = dto.ToEntity();

        context.Category.Add(category);
        await context.SaveChangesAsync();

        logger.LogInformation("Categoria criada com sucesso. Id: {CategoryId}", category.CategoryId);

        return category.ToDto();
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategories()
    {
        logger.LogInformation("Buscando todas as categorias");

        var categories = await context.Category.ToListAsync();

        logger.LogInformation("Foram encontradas {Count} categorias", categories.Count);

        return categories.Select(x => x.ToDto());
    }

    public async Task<CategoryDto> GetById(int id)
    {
        logger.LogInformation("Buscando categoria por Id: {CategoryId}", id);

        var category = await context.Category.FindAsync(id);

        if (category == null)
        {
            logger.LogWarning("Categoria não encontrada. Id: {CategoryId}", id);
            throw new InvalidOperationException($"Categoria com Id {id} não encontrada.");
        }

        return category.ToDto();
    }

    public async Task<CategoryDto> UpdateCategory(int id, CategoryInputDto dto)
    {
        logger.LogInformation("Atualizando categoria. Id: {CategoryId}", id);

        var existingCategory = await context.Category.FindAsync(id);

        if (existingCategory == null)
        {
            logger.LogWarning("Categoria não encontrada para atualização. Id: {CategoryId}", id);
            throw new InvalidOperationException($"Categoria com Id {id} não encontrada.");
        }

        existingCategory.UpdateEntity(dto);

        await context.SaveChangesAsync();

        logger.LogInformation("Categoria atualizada com sucesso. Id: {CategoryId}", id);

        return existingCategory.ToDto();
    }

    public async Task DeleteCategory(int id)
    {
        logger.LogInformation("Excluindo categoria. Id: {CategoryId}", id);

        var category = await context.Category.FindAsync(id);

        if (category == null)
        {
            logger.LogWarning("Categoria não encontrada para exclusão. Id: {CategoryId}", id);
            throw new InvalidOperationException($"Categoria com Id {id} não encontrada.");
        }

        context.Category.Remove(category);
        await context.SaveChangesAsync();

        logger.LogInformation("Categoria excluída com sucesso. Id: {CategoryId}", id);
    }
}