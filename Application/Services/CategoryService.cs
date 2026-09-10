using ApiDoces.Dtos.Category;
using ApiDoces.Mappings;
using Data;
using Microsoft.EntityFrameworkCore;

namespace ApiDoces.Services;

public class CategoryService(ApplicationDbContext context)
{
    public async Task CreateCategory(CreateCategoryDto dto)
    {
        var category = dto.ToEntity();

        context.Category.Add(category);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategories()
    {
        var categories = await context.Category.ToListAsync();
        return categories.Select(x => x.ToDto());
    }

    public async Task<CategoryDto?> GetById(int id)
    {
        var category = await context.Category.FindAsync(id);
        return category?.ToDto();
    }

    public async Task<CategoryDto?> UpdateCategory(int id, UpdateCategoryDto dto)
    {
        var existingCategory = await context.Category.FindAsync(id);

        if (existingCategory == null)
            return null;

        existingCategory.UpdateFromDto(dto);
        await context.SaveChangesAsync();

        return existingCategory.ToDto();
    }

    public async Task<bool> DeleteCategory(int id)
    {
        var category = await context.Category.FindAsync(id);

        if (category == null)
            return false;

        context.Category.Remove(category);
        await context.SaveChangesAsync();

        return true;
    }
}